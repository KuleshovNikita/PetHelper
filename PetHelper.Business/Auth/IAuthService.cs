using PetHelper.Domain;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        ClaimsPrincipal Login(AuthModel authModel);

        ClaimsPrincipal Register(UserModel userModel);
    }
}
