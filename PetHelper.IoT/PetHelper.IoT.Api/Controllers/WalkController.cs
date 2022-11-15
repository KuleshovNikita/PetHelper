using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Controllers;
using PetHelper.IoT.Business.Walks;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Api.Controllers
{
    public class WalkController : ResultingController
    {
        private readonly IWalkService _walkService;

        public WalkController(IWalkService walkService)
            => _walkService = walkService;

        [HttpPost("/{petId:guid}")]
        public async Task<ServiceResult<Empty>> StartWalk(Guid petId)
            => await RunWithServiceResult(async () =>
            {
                return await _walkService.SetWalkSettings(petId);
            });
    }
}
