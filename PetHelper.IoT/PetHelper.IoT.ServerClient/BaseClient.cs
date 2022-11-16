using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.Exceptions;
using PetHelper.ServiceResulting;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PetHelper.IoT.ServerClient
{
    public abstract class BaseClient
    {
        private readonly HttpClient _client;

        protected BaseClient(IConfiguration config)
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

        protected async Task<ServiceResult<TResponse>> Get<TResponse>(string route)
        { 
            var response = await _client.GetAsync(route);
            var content = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();

            if (!content!.IsSuccessful)
            {
                throw new FailedServerRequestException();
            }

            return content;
        }
    }
}
