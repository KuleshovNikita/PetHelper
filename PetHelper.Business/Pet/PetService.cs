using AutoMapper;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Pet
{
    public class PetService : DataAccessableService<PetModel>, IPetService
    {
        private readonly IMapper _mapper;

        public PetService(IMapper mapper, IRepository<PetModel> repo) : base(repo)
        {
            _mapper = mapper;
        }

        public async Task<ServiceResult<Empty>> AddPet(PetRequestModel petRequestModel, Guid userId)
        {
            var serviceResult = new ServiceResult<Empty>();

            if(petRequestModel == null || userId == Guid.Empty)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFound);
            }

            var petDomainModel = _mapper.Map<PetModel>(petRequestModel);
            petDomainModel.OwnerId = userId;

            var result = await _repository.Insert(petDomainModel);
            result.CatchAny();

            return new ServiceResult<Empty>().Success();
        }
    }
}
