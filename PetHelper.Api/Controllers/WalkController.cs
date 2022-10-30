using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.Business.Walk;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Authorize]
    public class WalkController : ResultingController
    {
        private readonly IWalkService _walkService;

        public WalkController(IWalkService walkService) => _walkService = walkService;

        [HttpPost("")]
        public async Task<ServiceResult<WalkModel>> StartWalk([FromBody] WalkRequestModel requestModel)
            => await RunWithServiceResult(async () => await _walkService.StartWalk(requestModel));

        [HttpGet("{petId:guid}")]
        public async Task<ServiceResult<IEnumerable<WalkModel>>> GetWalks(Guid petId)
            => await RunWithServiceResult(async () => await _walkService.GetWalks(x => x.PetId == petId));

        [HttpPatch("{walkId:guid}")]
        public async Task<ServiceResult<Empty>> FinishWalk(Guid walkId)
            => await RunWithServiceResult(async () => await _walkService.FinishWalk(walkId));
    }
}
