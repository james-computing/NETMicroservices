namespace HttpRequests
{
    internal class Program
    {
        public async static Task Main()
        {
            await CommandsServiceGet();
        }

        // Worked
        public async static Task CommandsServiceGet()
        {
            Console.WriteLine("Creating HTTP client...");
            HttpClient client = new HttpClient();
            Console.WriteLine("Making HTTP request...");
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://localhost:7178/api/CommandsForPlatforms");
                Console.WriteLine("Received response...");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Status: {response.StatusCode}");
                    string str = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {str}");
                }
                else
                {
                    Console.WriteLine($"Status: {response.StatusCode}");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}