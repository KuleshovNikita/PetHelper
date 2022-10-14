using PetHelper.Domain;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        public ClaimsPrincipal Login(AuthModel authModel);
    }
}
