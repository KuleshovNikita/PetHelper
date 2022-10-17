using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Business.Pet;
using PetHelper.ServiceResulting;
using System.Security.Claims;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ResultingController
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService) => _petService = petService;

        [HttpPost("addpet")]
        [Authorize]
        public async Task<ServiceResult<Empty>> AddPet([FromBody] PetRequestModel petModel)
            => await RunWithServiceResult(async () =>
            {
                var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

                await _petService.AddPet(petModel, new Guid(userId));

                return SuccessEmptyResult();
            });
    }
}
