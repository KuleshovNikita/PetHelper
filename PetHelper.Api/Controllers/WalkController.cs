using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.Business.Pet;
using PetHelper.Business.Walk;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Authorize]
    public class WalkController : ResultingController
    {
        private readonly IWalkService _walkService;
        private readonly IPetService _petService;

        public WalkController(IWalkService walkService, IPetService petService)
        {
            _walkService = walkService;
            _petService = petService;
        }

        [HttpPost("")]
        public async Task<ServiceResult<WalkModel>> StartWalk([FromBody] WalkRequestModel requestModel)
            => await RunWithServiceResult(async () =>
            {
                var walkResult = await _walkService.StartWalk(requestModel);
                await _petService.UpdatePet(new PetUpdateRequestModel { IsWalking = true }, walkResult.Value.PetId);

                return walkResult;
            });

        [HttpGet("{petId:guid}")]
        public async Task<ServiceResult<IEnumerable<WalkModel>>> GetWalks(Guid petId)
            => await RunWithServiceResult(async () => await _walkService.GetWalks(x => x.PetId == petId));

        [HttpPatch("{walkId:guid}")]
        public async Task<ServiceResult<WalkModel>> FinishWalk(Guid walkId)
            => await RunWithServiceResult(async () =>
            {
                var walkResult = await _walkService.FinishWalk(walkId);
                await _petService.UpdatePet(new PetUpdateRequestModel { IsWalking = false }, walkResult.Value.PetId);

                return walkResult;
            });
    }
}
