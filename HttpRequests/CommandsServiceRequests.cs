using System;
using System.Collections.Generic;
using System.Text;

namespace HttpRequests
{
    internal class CommandsServiceRequests
    {
        // Worked
        public async static Task GetAsync()
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Worked
        public async static Task PostAsync()
        {
            Console.WriteLine("Creating HTTP client...");
            HttpClient client = new HttpClient();
            Console.WriteLine("Creating content of POST request");
            HttpContent content = new StringContent("nothing");
            Console.WriteLine("Making HTTP request...");
            try
            {
                HttpResponseMessage response = await client.PostAsync("https://localhost:7178/api/CommandsForPlatforms", content);
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
