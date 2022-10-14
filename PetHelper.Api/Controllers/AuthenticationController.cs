using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Business.Auth;
using PetHelper.Domain;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthenticationController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<AuthModel> Login([FromBody] AuthModel authModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidDataException("invalid data found");
            }

            var claims = _authService.Login(authModel);

            if(!claims.Claims.Any())
            {
                throw new Exception("No such user exists");
            }

            await HttpContext.SignInAsync(claims);

            return authModel;
            //TODO выполнить запрос к БД на аунтификацию, если успешно - перенаправить по returnUrl
        }
    }
}
