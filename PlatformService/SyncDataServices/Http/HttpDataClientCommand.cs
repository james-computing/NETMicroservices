using PlatformService.DTOs;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpDataClientCommand : ICommandDataClient
    {

        private readonly HttpClient _httpClient;
        //private readonly IConfiguration _configuration;
        private readonly string _commandsServiceUrl;


        public HttpDataClientCommand(HttpClient httpClient, IConfiguration configuration, IHostEnvironment environment)
        {
            _httpClient = httpClient;
            //_configuration = configuration;

            if (environment.IsDevelopment())
            {
                _commandsServiceUrl = configuration.GetValue<string>("CommandsServiceDev")!;
            }
            else
            {
                _commandsServiceUrl = configuration.GetValue<string>("CommandsServiceProd")!;
            }
        }

        public async Task SendPlatformToCommand(PlatformReadDTO platformReadDTO)
        {
            HttpContent httpContent = new StringContent(
                JsonSerializer.Serialize(platformReadDTO),
                Encoding.UTF8,
                "application/json"
                );

            HttpResponseMessage response = await _httpClient.PostAsync(
                $"{_commandsServiceUrl}/api/CommandsForPlatforms",
                httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService was NOT OK!");
            }
        }
    }
}
