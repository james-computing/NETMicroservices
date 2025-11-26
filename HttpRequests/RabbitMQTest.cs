using RabbitMQ.Client;

namespace HttpRequests
{
    internal class RabbitMQTest
    {
        public async Task Publish()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "default_user_5DQWlxMpiLRz842dNjP",
                Password = "fF5wucx8NqEFST3ue_JT-O71dL3girKT"
            };

            Console.WriteLine("Creating connection...");
            IConnection connection = await connectionFactory.CreateConnectionAsync();
            Console.WriteLine("Creating channel...");
            IChannel channel = await connection.CreateChannelAsync();

            Console.WriteLine("yes");
        }

        // Succeded
        public async Task GetManagerPage()
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:15672");
            if (response.IsSuccessStatusCode)
            {
                string str = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{str}");
            }
            else
            {
                Console.WriteLine($"Failed: {response.StatusCode}");
            }
        }
    }
}
