using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.User
{
    public interface IUserService
    {
        Task<ServiceResult<Empty>> AddUser(UserModel userModel);

        Task<ServiceResult<UserModel>> GetUser(Expression<Func<UserModel, bool>> predicate, string messageIfNotFound);

        Task<ServiceResult<Empty>> UpdateUser(UserModel userModel);
    }
}
