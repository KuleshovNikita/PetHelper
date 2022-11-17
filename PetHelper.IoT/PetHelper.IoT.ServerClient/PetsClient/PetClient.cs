using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.PetModels;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.ServerClient.PetsClient
{
    public class PetClient : BaseClient, IPetClient
    {
        public PetClient(IConfiguration config) : base(config)
        {
        }

        public async Task<ServiceResult<PetModel>> GetPet(Guid petId) => await Get<PetModel>($"/api/pet/{petId}");
    }
}
