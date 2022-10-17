using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Pet
{
    public interface IPetService
    {
        Task<ServiceResult<Empty>> AddPet(PetRequestModel petRequestModel, Guid userId);
    }
}
