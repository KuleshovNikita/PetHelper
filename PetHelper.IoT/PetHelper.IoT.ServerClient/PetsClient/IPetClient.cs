using PetHelper.IoT.Domain.PetModels;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.PetsClient
{
    public interface IPetClient
    {
        Task<ServiceResult<PetModel>> GetPet(Guid petId);
    }
}
