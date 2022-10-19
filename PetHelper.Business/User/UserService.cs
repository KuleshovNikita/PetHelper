using AutoMapper;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Extensions;
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
        private readonly IMapper _mapper;

        public UserService(IMapper mapper, IPasswordHasher passwordHasher, IRepository<UserModel> repository) 
            : base(repository)
        {
            _mapper = mapper;
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
                var userModel = await GetUser(x => x.Id == userId);
                (await _repository.Remove(userModel.Value)).CatchAny();

                return serviceResult.Success();
            }

            return serviceResult.FailAndThrow(Resources.TheItemDoesntExist);
        }

        public async Task<ServiceResult<UserModel>> GetUser(Expression<Func<UserModel, bool>> predicate)
        {
            var result = await _repository.FirstOrDefault(predicate);

            return result.Catch<EntityNotFoundException>(Resources.TheItemDoesntExist)
                         .CatchAny();
        }

        public async Task<ServiceResult<Empty>> UpdateUser(UserUpdateRequestModel userModel, Guid userId)
        {
            userModel.Id = userId;
            var user = await GetUser(x => x.Id == userModel.Id);
            user.Value = _mapper.MapOnlyUpdatedProperties(userModel, user.Value);

            var result = await _repository.Update(user.Value);
            return result.CatchAny();
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
