using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Business.Pet;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ResultingController
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService) => _petService = petService;

        [HttpPost("addPet")]
        [Authorize]
        public async Task<ServiceResult<Empty>> AddPet([FromBody] PetRequestModel petModel)
            => await RunWithServiceResult(async () =>
            {
                var userId = GetUserIdFromToken();

                return await _petService.AddPet(petModel, userId);
            });

        [HttpPut("updatePet/{petId:guid}")]
        [Authorize]
        public async Task<ServiceResult<Empty>> UpdatePet(Guid petId, [FromBody] PetUpdateRequestModel petModel)
            => await RunWithServiceResult(async () => await _petService.UpdatePet(petModel, petId));

        [HttpGet("getPet/{petId:guid}")]
        [Authorize]
        public async Task<ServiceResult<PetModel>> GetPet(Guid petId)
            => await RunWithServiceResult(async () => await _petService.GetPet(x => x.Id == petId));

        [HttpDelete("removePet/{petId:guid}")]
        [Authorize]
        public async Task<ServiceResult<Empty>> RemovePet(Guid petId)
            => await RunWithServiceResult(async () => await _petService.RemovePet(petId));
    }
}
