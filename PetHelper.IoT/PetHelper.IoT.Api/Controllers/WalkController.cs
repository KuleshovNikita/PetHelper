using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Controllers;
using PetHelper.IoT.Business.Walks;
using PetHelper.IoT.Domain.WalksModels;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Api.Controllers
{
    public class WalkController : ResultingController
    {
        private readonly IWalkService _walkService;

        public WalkController(IWalkService walkService)
            => _walkService = walkService;

        [HttpPost]
        public async Task<ServiceResult<Empty>> StartWalk(WalkStartInfo walkStartInfo)
            => await RunWithServiceResult(async () => await _walkService.SetWalkSettings(walkStartInfo));


    }
}
