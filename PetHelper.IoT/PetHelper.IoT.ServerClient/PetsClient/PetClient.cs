using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.PetModels;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.PetsClient
{
    public class PetClient : IPetClient
    {
        private readonly ServerClient _client;

        public PetClient(ServerClient client)
            => _client = client;

        public async Task<ServiceResult<PetModel>> GetPet(Guid petId) 
            => await _client.Get<PetModel>($"/api/pet/{petId}");
    }
}
