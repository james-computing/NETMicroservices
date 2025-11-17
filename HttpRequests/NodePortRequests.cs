using System;
using System.Collections.Generic;
using System.Text;

namespace HttpRequests
{
    internal class NodePortRequests
    {
        public async static Task GetAsync()
        {
            Console.WriteLine("Creating HTTP client...");
            HttpClient client = new HttpClient();
            Console.WriteLine("Making HTTP request...");
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:30722/api/Platforms");
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
