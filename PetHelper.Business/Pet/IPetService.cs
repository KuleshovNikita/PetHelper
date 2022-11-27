using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Pet
{
    public interface IPetService
    {
        Task<ServiceResult<Empty>> AddPet(PetRequestModel petRequestModel, Guid userId);

        Task<ServiceResult<Empty>> UpdatePet(PetUpdateRequestModel petModel, Guid petId);

        Task<ServiceResult<PetModel>> GetPet(Expression<Func<PetModel, bool>> predicate);

        Task<ServiceResult<IEnumerable<PetModel>>> GetPets(Expression<Func<PetModel, bool>> predicate);

        Task<ServiceResult<Empty>> RemovePet(Guid petId);
    }
}
