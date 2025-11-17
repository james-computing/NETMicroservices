using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace HttpRequests
{
    internal class PlatformsServiceRequests
    {
        public async static Task PostAsync()
        {
            Console.WriteLine("Creating HTTP client...");
            HttpClient client = new HttpClient();
            Console.WriteLine("Making HTTP request...");
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(
                    "https://localhost:7241/api/Platforms",
                    new
                    {
                        Name = "name",
                        Publisher = "publisher",
                        Cost = "cost"
                    }
                    );
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
