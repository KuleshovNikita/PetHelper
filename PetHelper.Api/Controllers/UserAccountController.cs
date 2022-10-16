using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetHelper.Business.User;
using PetHelper.Domain;
using PetHelper.Domain.Properties;
using PetHelper.ServiceResulting;
using System.Web;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserAccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getUser/{userId:guid}")]
        [Authorize]
        public async Task<ServiceResult<UserModel>> GetUser(Guid userId)
        {
            var result = new ServiceResult<UserModel>();

            try
            {
                result = await _userService.GetUser(
                            predicate: x => x.Id == userId,
                            messageIfNotFound: Resources.TheItemDoesntExist);

                return result.Success();
            }
            catch (FailedServiceResultException ex)
            {
                return result.Fail(ex);
            }
        }
    }
}
