using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.User;
using PetHelper.Domain;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ResultingController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpGet("getUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<UserModel>> GetUser(Guid userId)
            => await RunWithServiceResult(async () => await _userService.GetUser(x => x.Id == userId));

        [HttpPut("updateUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<Empty>> UpdateUser(Guid userId, [FromBody] UserUpdateRequestModel userRequestModel)
            => await RunWithServiceResult(async () =>
            {
                userRequestModel.Id = userId;
                return await _userService.UpdateUser(userRequestModel);
            });

        [HttpDelete("removeUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<Empty>> RemoveUser(Guid userId)
            => await RunWithServiceResult(async () => await _userService.RemoveUser(userId));
    }
}
