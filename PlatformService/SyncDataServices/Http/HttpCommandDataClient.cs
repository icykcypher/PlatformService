using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient(HttpClient client, IConfiguration configuration) : ICommandDataClient
    {
        private readonly HttpClient _client = client;
        private readonly IConfiguration _config = configuration;

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(JsonSerializer.Serialize(platform), encoding: Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_config["CommandService"]}/api/c/platforms", httpContent);
            if (response.IsSuccessStatusCode) Console.WriteLine("--> Sync POST to CommandsService was OK!");
            else Console.WriteLine("--> Sync POST to CommandsService wasn`t OK!");
        }
    }
}