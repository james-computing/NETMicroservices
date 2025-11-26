using PlatformService.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PlatformService.DataServices.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private IConnection? _connection;
        private IChannel? _channel;
        private const string exchange = "trigger";

        public async Task<bool> InitializeRabbitMQ(IHostEnvironment environment, IConfiguration configuration)
        {
            string hostName;
            int port;

            if (environment.IsDevelopment())
            {
                hostName = configuration.GetValue<string>("RabbitMQHostDev")!;
            }
            else
            {
                hostName = configuration.GetValue<string>("RabbitMQHostProd")!;
            }
            port = configuration.GetValue<int>("RabbitMQPort")!;

            Console.WriteLine($"RabbitMQ hostName: {hostName}.");
            Console.WriteLine($"RabbitMQ port: {port}.");

            string userName = configuration.GetValue<string>("RabbitMQUserName")!;
            string password = configuration.GetValue<string>("RabbitMQPassword")!;
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

                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to message bus");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus: {ex.Message}");
            }
            return false;
        }
        
        private async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public async Task PublishNewPlatform(PlatformPublishedDTO platformPublishedDTO)
        {
            Console.WriteLine("--> RabbitMQ Connecition Open, sending message...");
            string message = JsonSerializer.Serialize(platformPublishedDTO);
            await SendMessage(message);
        }

        private async Task SendMessage(string message)
        {
            byte[] body = Encoding.UTF8.GetBytes(message);
            const string noRoutingKey = "";
            await _channel!.BasicPublishAsync(exchange: exchange, routingKey: noRoutingKey, body: body);
            Console.WriteLine($"--> We have sent message: {message}");
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection!.IsOpen)
            {
                await _connection!.CloseAsync();
            }
        }
    }
}
