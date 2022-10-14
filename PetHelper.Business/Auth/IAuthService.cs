using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<ClaimsPrincipal>> Login(AuthModel authModel);

        Task<ServiceResult<ClaimsPrincipal>> Register(UserModel userModel);
    }
}
