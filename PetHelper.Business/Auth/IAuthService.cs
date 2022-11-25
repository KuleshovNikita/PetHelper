using PetHelper.Api.Models.RequestModels;
using PetHelper.Domain;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> ConfirmEmail(string key);

        Task<ServiceResult<UserModel>> GetCurrentUser();

        Task<ServiceResult<string>> Login(AuthModel authModel);

        Task<ServiceResult<string>> Register(UserRequestModel userModel);
    }
}
