using AutoMapper;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Business.User;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;

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

            var petOwnerModel = await _userService.GetUser(x => x.Id == userId, Resources.TheItemDoesntExist);

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
    }
}
