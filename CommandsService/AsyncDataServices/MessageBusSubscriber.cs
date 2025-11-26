
using CommandsService.EventProcessing;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService, IAsyncDisposable
    {
        IHostEnvironment _environment;
        IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private const string exchange = "trigger";

        private IConnection? _connection;
        private IChannel? _channel;
        private string? _queueName;

        public MessageBusSubscriber(
            IHostEnvironment environment,
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _environment = environment;
            _configuration = configuration;
            _eventProcessor = eventProcessor;
        }

        private async Task<bool> InitializeRabbitMQ()
        {
            string hostName;
            int port;

            if (_environment.IsDevelopment())
            {
                hostName = _configuration.GetValue<string>("RabbitMQHostDev")!;
            }
            else
            {
                hostName = _configuration.GetValue<string>("RabbitMQHostProd")!;
            }
            port = _configuration.GetValue<int>("RabbitMQPort")!;

            Console.WriteLine($"RabbitMQ hostName: {hostName}");
            Console.WriteLine($"RabbitMQ port: {port}");

            string userName = _configuration.GetValue<string>("RabbitMQUserName")!;
            string password = _configuration.GetValue<string>("RabbitMQPassword")!;
            Console.WriteLine("Creating RabbitMQ ConnectionFactory...");
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password,
            };

            try
            {
                Console.WriteLine("Creating RabbitMQ connection...");
                _connection = await connectionFactory.CreateConnectionAsync();
                Console.WriteLine("Creating RabbitMQ channel...");
                _channel = await _connection.CreateChannelAsync();
                await _channel.ExchangeDeclareAsync(exchange: exchange, type: ExchangeType.Fanout);

                Console.WriteLine("Creating RabbitMQ queue...");
                QueueDeclareOk queueOk = await _channel.QueueDeclareAsync();
                _queueName = queueOk.QueueName;
                const string noRoutingKey = "";
                await _channel.QueueBindAsync(queue: _queueName, exchange: exchange, routingKey: noRoutingKey);

                Console.WriteLine("--> Listening on the message bus");

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Failed to connect to RabbitMQ: {ex.Message}");
            }
            return false;
        }

        private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            bool successfulConnection = await InitializeRabbitMQ();
            if(successfulConnection == false)
            {
                return;
            }

            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel!);
            consumer.ReceivedAsync += async (object sender, BasicDeliverEventArgs eventArgs) =>
            {
                Console.WriteLine("--> Event Received");

                byte[] body = eventArgs.Body.ToArray();
                string notificationMessage = Encoding.UTF8.GetString(body);
                _eventProcessor.ProcessEvent(notificationMessage);
            };

            await _channel!.BasicConsumeAsync(queue: _queueName!, autoAck: true, consumer: consumer);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection!.IsOpen)
            {
                await _connection!.CloseAsync();
            }
            Dispose();
        }
    }
}
