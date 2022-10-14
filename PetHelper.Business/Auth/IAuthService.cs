using PetHelper.Domain;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        Task<ClaimsPrincipal> Login(AuthModel authModel);

        Task<ClaimsPrincipal> Register(UserModel userModel);
    }
}
