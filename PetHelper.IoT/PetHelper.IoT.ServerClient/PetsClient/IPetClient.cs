using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.PetsClient
{
    public interface IPetClient
    {
        Task<ServiceResult<Empty>> GetAllowedPetDistance(Guid petId);
    }
}
