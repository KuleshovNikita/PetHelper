using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PetHelper.IoT.Domain.Exceptions;
using PetHelper.ServiceResulting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJNeWt5dGEgS3VsZXNob3YiLCJzdWIiOiI4NjBmNDVjYi1iZTg2LTRhZWQtOGJiOS00ZTU0YzYwZjlmNmQiLCJlbWFpbCI6Im15a3l0YS5rdWxlc2hvdkBudXJlLnVhIiwiZXhwIjoxNjY4NjkzMzEyLCJpc3MiOiJQZXRIZWxwZXJJc3N1ZXIiLCJhdWQiOiJQZXRIZWxwZXJBdWRpZW5jZSJ9.JtOggwxE_VrAuEytYZtltM5HgENtMFHf9OnWHgsARNU");
            var response = await _client.GetAsync(route);
            var content = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();

            return content;
        }
    }
}
