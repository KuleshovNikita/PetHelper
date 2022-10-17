using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;
using System.Reflection;

namespace PetHelper.Business.User
{
    public class UserService : DataAccessableService<UserModel>, IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IPasswordHasher passwordHasher, IRepository<UserModel> repository) 
            : base(repository)
        {
            _passwordHasher = passwordHasher;
            _mapper = mapper;
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

            (await _repository.Insert(userModel))
                .Catch<OperationCanceledException>()
                .Catch<DbUpdateException>()
                .Catch<DbUpdateConcurrencyException>();

            return serviceResult.Success();
        }

        public async Task<ServiceResult<Empty>> RemoveUser(Guid userId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (UserExists(userId, out var userModel))
            {
                (await _repository.Remove(userModel))
                    .Catch<OperationCanceledException>()
                    .Catch<DbUpdateException>()
                    .Catch<DbUpdateConcurrencyException>();

                return serviceResult.Success();
            }

            return serviceResult.FailAndThrow(Resources.TheItemDoesntExist);
        }

        public async Task<ServiceResult<UserModel>> GetUser(
            Expression<Func<UserModel, bool>> predicate, string messageIfNotFound)
                => (await _repository.FirstOrDefault(predicate))
                        .Catch<EntityNotFoundException>(messageIfNotFound)
                        .CatchAny();

        public async Task<ServiceResult<Empty>> UpdateUser(UserUpdateRequestModel userModel)
        {
            var user = await GetUser(x => x.Id == userModel.Id, Resources.TheItemDoesntExist);

            var propertiesToUpdate = userModel.GetType().GetProperties().Where(x => x.GetValue(userModel) != null).ToList();
            var userModelProps = user.Value.GetType().GetProperties();

            foreach (var prop in propertiesToUpdate)
            {
                var propToUpdate = userModelProps.First(x => x.Name == prop.Name);
                propToUpdate.SetValue(user.Value, prop.GetValue(userModel));
            }

            return (await _repository.Update(user.Value)).CatchAny();
        }

        private bool UserExists(Guid userId, out UserModel userModel)
        {
            try
            {
                userModel = GetUser(x => x.Id == userId, Resources.TheItemDoesntExist).Result.Value;
                return true;
            }
            catch (FailedServiceResultException)
            {
                userModel = new UserModel();
                return false;
            }
        }

        private async Task<bool> IsLoginAlreadyRegistered(string login)
        {
            var serviceResult = (await _repository.Any(x => x.Login == login))
                                    .Catch<OperationCanceledException>()
                                    .Catch<ArgumentNullException>();

            return serviceResult.Value;
        }
    }
}
