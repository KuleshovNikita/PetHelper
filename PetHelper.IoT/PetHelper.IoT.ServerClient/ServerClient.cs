using Microsoft.Extensions.Configuration;
using PetHelper.ServiceResulting;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PetHelper.IoT.ServerClient
{
    public class ServerClient
    {
        private readonly HttpClient _client;

        public ServerClient(IConfiguration config)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(config.GetSection("ServerUrl").Value!),
                Timeout = new TimeSpan(
                            hours: 0,
                            minutes: 0, 
                            seconds: int.Parse(config.GetSection("ServerRequestTimeOutSeconds").Value!))
            };

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public void SetToken(string token)
            => _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        public async Task<ServiceResult<TResponse>> Get<TResponse>(string route)
        {
            var response = await _client.GetAsync(route);

            var content = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();
            return content!;
        }

        public async Task<ServiceResult<TResponse>> Post<TBody, TResponse>(string route, TBody body)
        {
            var response = await _client.PostAsync(route, JsonContent.Create(body));

            var content = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();
            return content!;

        }

        public async Task<ServiceResult<Empty>> Patch(string route)
        {
            var response = await _client.PatchAsync(route, null);

            var content = await response.Content.ReadFromJsonAsync<ServiceResult<Empty>>();
            return content!;
        }
    }
}
