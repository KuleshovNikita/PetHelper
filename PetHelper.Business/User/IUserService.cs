using PetHelper.Api.Models.RequestModels;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.User
{
    public interface IUserService
    {
        Task<ServiceResult<Empty>> AddUser(UserModel userModel);

        Task<ServiceResult<UserModel>> GetUser(Expression<Func<UserModel, bool>> predicate);

        Task<ServiceResult<Empty>> UpdateUser(UserUpdateRequestModel userModel, Guid userId);

        Task<ServiceResult<Empty>> RemoveUser(Guid userId);
    }
}
