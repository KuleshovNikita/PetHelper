using PetHelper.Api.Models.RequestModels;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> ConfirmEmail(string key);

        Task<ServiceResult<string>> Login(AuthModel authModel);

        Task<ServiceResult<string>> Register(UserRequestModel userModel);
    }
}
