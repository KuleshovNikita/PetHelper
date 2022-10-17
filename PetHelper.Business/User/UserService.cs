using AutoMapper;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.User
{
    public class UserService : DataAccessableService<UserModel>, IUserService
    {
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IPasswordHasher passwordHasher, IRepository<UserModel> repository) 
            : base(repository)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult<Empty>> AddUser(UserModel userModel)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (await IsLoginAlreadyRegistered(userModel.Login))
            {
                return serviceResult.FailAndThrow(Resources.TheLoginIsAlreadyRegistered);
            }

            var hashedPassword = _passwordHasher.HashPassword(userModel.Password);
            userModel.Password = hashedPassword;
            userModel.Id = Guid.NewGuid();

            (await _repository.Insert(userModel)).CatchAny();

            return serviceResult.Success();
        }

        public async Task<ServiceResult<Empty>> RemoveUser(Guid userId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (await UserExists(userId))
            {
                var userModel = await GetUser(x => x.Id == userId, Resources.TheItemDoesntExist);
                (await _repository.Remove(userModel.Value)).CatchAny();

                return serviceResult.Success();
            }

            return serviceResult.FailAndThrow(Resources.TheItemDoesntExist);
        }

        public async Task<ServiceResult<UserModel>> GetUser(Expression<Func<UserModel, bool>> predicate, string messageIfNotFound)
        {
            var result = await _repository.FirstOrDefault(predicate);

            return result.Catch<EntityNotFoundException>(messageIfNotFound)
                         .CatchAny();
        }

        public async Task<ServiceResult<Empty>> UpdateUser(UserUpdateRequestModel userModel)
        {
            var user = await GetUser(x => x.Id == userModel.Id, Resources.TheItemDoesntExist);
            user.Value = MapOnlyUpdatedProperties(userModel, user.Value);

            var result = await _repository.Update(user.Value);
            return result.CatchAny();
        }

        private UserModel MapOnlyUpdatedProperties(UserUpdateRequestModel from, UserModel to)
        {
            var propertiesToUpdate = from.GetType()
                                         .GetProperties()
                                         .Where(x => x.GetValue(from) != null)
                                         .ToList();

            var userModelProps = to.GetType().GetProperties();

            foreach (var prop in propertiesToUpdate)
            {
                var propToUpdate = userModelProps.First(x => x.Name == prop.Name);
                propToUpdate.SetValue(to, prop.GetValue(from));
            }

            return to;
        }

        private async Task<bool> UserExists(Guid userId)
        {
            var result = await _repository.Any(x => x.Id == userId);
            return result.CatchAny().Value;
        }

        private async Task<bool> IsLoginAlreadyRegistered(string login)
        {
            var result = await _repository.Any(x => x.Login == login);
            return result.CatchAny().Value;
        }
    }
}
