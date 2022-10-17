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

        public AuthenticationController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<ServiceResult<Empty>> Register([FromBody] UserRequestModel userModel)
            => await RunWithServiceResult(async () =>
            {
                var claimsResult = await _authService.Register(userModel);
                await HttpContext.SignInAsync(claimsResult.Value);

                return SuccessEmptyResult();
            });

        [HttpPost("login")]
        public async Task<ServiceResult<Empty>> Login([FromBody] AuthModel authModel)
            => await RunWithServiceResult(async () =>
            {
                var claimsResult = await _authService.Login(authModel);
                await HttpContext.SignInAsync(claimsResult.Value);

                return SuccessEmptyResult();
            });

        [HttpGet("confirmEmail/{key}")]
        public async Task<ServiceResult<Empty>> ConfirmEmail(string key)
            => await RunWithServiceResult(async () =>
            {
                key = Uri.UnescapeDataString(key);
                var newClaims = await _authService.ConfirmEmail(key);

                await HttpContext.SignInAsync(newClaims.Value);

                return SuccessEmptyResult();
            });

        [Authorize]
        [HttpGet("logout")]
        public async Task<ServiceResult<Empty>> LogOut()
            => await RunWithServiceResult(async () =>
            {
                await HttpContext.SignOutAsync();

                return SuccessEmptyResult();
            });
    }
}
