using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels;
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
        private readonly IMapper _mapper;

        public UserAccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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

        [HttpPut("updateUser")]
        [Authorize]
        public async Task<ServiceResult<Empty>> UpdateUser([FromBody] UserRequestModel userRequestModel)
            => await RunWithServiceResult(async () =>
            {
                var userDomainModel = _mapper.Map<UserModel>(userRequestModel);
                await _userService.UpdateUser(userDomainModel);

                return SuccessEmptyResult();
            });

        [HttpPut("removeUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<Empty>> RemoveUser(Guid userId)
            => await RunWithServiceResult(async () =>
            {
                await _userService.RemoveUser(userId);

                return SuccessEmptyResult();
            });
    }
}
