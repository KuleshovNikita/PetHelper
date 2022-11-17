using PetHelper.IoT.Domain.Auth;
using PetHelper.IoT.ServerClient.Auth;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthClient _authClient;

        public AuthService(IAuthClient authClient)
        {
            _authClient = authClient;
        }

        public async Task<ServiceResult<Empty>> Login(AuthModel authModel)
        {
            var result = await _authClient.Login(authModel);
            return result.CatchAny();
        }
    }
}
