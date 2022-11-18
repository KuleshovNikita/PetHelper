using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.Auth;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.Auth
{
    public class AuthClient : IAuthClient
    {
        private readonly ServerClient _client;

        public AuthClient(ServerClient client)
            => _client = client;

        public async Task<ServiceResult<Empty>> Login(AuthModel authModel)
        {
            var tokenResult = await _client.Post<AuthModel, string>("/api/authentication/login", authModel);
            tokenResult.CatchAny();

            _client.SetToken(tokenResult.Value);

            return new ServiceResult<Empty>();
        }
    }
}
