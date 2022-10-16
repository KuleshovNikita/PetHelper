using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Auth;
using PetHelper.Domain;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ResultingController
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
            => await RunWithServiceResult(async () =>
            {
                var claimsResult = await _authService.Register(_mapper.Map<UserModel>(userModel));
                await HttpContext.SignInAsync(claimsResult.Value);

                return new ServiceResult<Empty>().Success();
            });

        [HttpPost("login")]
        public async Task<ServiceResult<Empty>> Login([FromBody] AuthModel authModel)
            => await RunWithServiceResult(async () =>
            {
                var claimsResult = await _authService.Login(authModel);
                await HttpContext.SignInAsync(claimsResult.Value);

                return new ServiceResult<Empty>().Success();
            });

        [HttpGet("confirmEmail/{key}")]
        public async Task<ServiceResult<Empty>> ConfirmEmail(string key)
            => await RunWithServiceResult(async () =>
            {
                key = Uri.UnescapeDataString(key);
                await _authService.ConfirmEmail(key);

                return new ServiceResult<Empty>().Success();
            });

        [Authorize]
        [HttpGet("logout")]
        public async Task<ServiceResult<Empty>> LogOut()
            => await RunWithServiceResult(async () =>
            {
                await HttpContext.SignOutAsync();

                return new ServiceResult<Empty>().Success();
            });
    }
}
