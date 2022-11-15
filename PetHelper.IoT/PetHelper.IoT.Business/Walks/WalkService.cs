using PetHelper.IoT.ServerClient.PetsClient;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Business.Walks
{
    public class WalkService : IWalkService
    {
        private readonly IPetClient _petClient;

        public WalkService(IPetClient petClient)
            => _petClient = petClient;

        public Task<ServiceResult<Empty>> SetWalkSettings(Guid petId)
        {
            var result = _petClient.GetAllowedPetDistance(petId);
        }
    }
}
