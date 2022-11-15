using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.PetsClient
{
    public class PetClient : IPetClient
    {
        public async Task<ServiceResult<Empty>> GetAllowedPetDistance(Guid petId)
        {

        }
    }
}
