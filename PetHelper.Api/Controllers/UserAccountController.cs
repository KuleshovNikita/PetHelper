using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Business.User;
using PetHelper.Domain;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ResultingController
    {
        private readonly IUserService _userService;

        public UserAccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<UserModel>> GetUser(Guid userId)
            => await RunWithServiceResult(async () =>
            {
                return await _userService.GetUser(
                                predicate: x => x.Id == userId,
                                messageIfNotFound: Resources.TheItemDoesntExist);
            });
    }
}
