using AutoMapper;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Business.Extensions;
using PetHelper.Business.User;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Pet
{
    public class PetService : DataAccessableService<PetModel>, IPetService
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public PetService(IUserService userService, IMapper mapper, IRepository<PetModel> repo) : base(repo)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ServiceResult<Empty>> AddPet(PetRequestModel petRequestModel, Guid userId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if(petRequestModel == null || userId == Guid.Empty)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFound);
            }

            var petOwnerModel = await _userService.GetUser(x => x.Id == userId);

            if(!petOwnerModel.Value.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow(Resources.EmailConfirmationIsNeeded);
            }

            var petDomainModel = _mapper.Map<PetModel>(petRequestModel);
            petDomainModel.OwnerId = userId;

            var result = await _repository.Insert(petDomainModel);
            result.CatchAny();

            return serviceResult.Success();
        }

        public async Task<ServiceResult<PetModel>> GetPet(Expression<Func<PetModel, bool>> predicate)
        {
            var result = await _repository.FirstOrDefault(predicate);
            return result.Catch<EntityNotFoundException>(Resources.TheItemDoesntExist)
                         .CatchAny();
        }

        public async Task<ServiceResult<Empty>> UpdatePet(PetUpdateRequestModel petRequestModel, Guid petId)
        {
            var petModel = await GetPet(x => x.Id == petId);
            petModel.Value = _mapper.MapOnlyUpdatedProperties(petRequestModel, petModel.Value);

            var result = await _repository.Update(petModel.Value);
            return result.CatchAny();
        }

        public async Task<ServiceResult<Empty>> RemovePet(Guid petId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if (await PetExists(petId))
            {
                var userModel = await GetPet(x => x.Id == petId);
                var removeResult = await _repository.Remove(userModel.Value);
                removeResult.CatchAny();

                return serviceResult.Success();
            }

            return serviceResult.FailAndThrow(Resources.TheItemDoesntExist);
        }

        private async Task<bool> PetExists(Guid petId)
        {
            var result = await _repository.Any(x => x.Id == petId);
            return result.CatchAny().Value;
        }
    }
}
