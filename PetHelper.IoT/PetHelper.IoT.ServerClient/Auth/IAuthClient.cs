using PetHelper.IoT.Domain.Auth;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.Auth
{
    public interface IAuthClient
    {
        Task<ServiceResult<Empty>> Login(AuthModel authModel);
    }
}
