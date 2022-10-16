using Microsoft.EntityFrameworkCore;
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

            (await _repository.Insert(userModel))
                .Catch<OperationCanceledException>()
                .Catch<DbUpdateException>()
                .Catch<DbUpdateConcurrencyException>();

            return serviceResult.Success();
        }

        public async Task<ServiceResult<UserModel>> GetUser(
            Expression<Func<UserModel, bool>> predicate, string messageIfNotFound)
                => (await _repository.FirstOrDefault(predicate))
                        .Catch<EntityNotFoundException>(messageIfNotFound)
                        .Catch<ArgumentNullException>()
                        .Catch<InvalidOperationException>()
                        .Catch<OperationCanceledException>();

        public async Task<ServiceResult<Empty>> UpdateUser(UserModel userModel)
            => (await _repository.Update(userModel))
                    .Catch<OperationCanceledException>()
                    .Catch<DbUpdateException>()
                    .Catch<DbUpdateConcurrencyException>();

        private async Task<bool> IsLoginAlreadyRegistered(string login)
        {
            var serviceResult = (await _repository.Any(x => x.Login == login))
                                    .Catch<OperationCanceledException>()
                                    .Catch<ArgumentNullException>();

            return serviceResult.Value;
        }
    }
}
