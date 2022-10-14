using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Auth;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Security.Claims;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ServiceResult<Empty>> Register([FromBody] UserRequestModel userModel)
            => await RunWithServiceResult(() => _authService.Register(_mapper.Map<UserModel>(userModel)));

        [HttpPost("login")]
        public async Task<ServiceResult<Empty>> Login([FromBody] AuthModel authModel)
            => await RunWithServiceResult(() => _authService.Login(authModel));

        private async Task<ServiceResult<Empty>> RunWithServiceResult(Func<Task<ServiceResult<ClaimsPrincipal>>> command)
        {
            var finalResult = new ServiceResult<Empty>();

            if (!ModelState.IsValid)
            {
                return finalResult.Fail("Invalid data found");
            }

            try
            {
                var claimsResult = await command();

                if (!claimsResult.IsSuccessful)
                {
                    return finalResult.Fail(claimsResult.ClientErrorMessage!);
                }

                await HttpContext.SignInAsync(claimsResult.Value);

                return finalResult.Success();
            }
            catch (FailedServiceResultException ex)
            {
                return finalResult.Fail(ex.Message);
            }
        }
    }
}
