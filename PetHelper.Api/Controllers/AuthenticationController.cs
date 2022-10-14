using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.RequestModels;
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

        [HttpPost("register")]
        public async Task<bool> Register([FromBody] UserRequestModel userModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidDataException("invalid data found");
            }

            //TODO вынести конфигурацию в DI
            var map = new MapperConfiguration(x => x.CreateMap<UserRequestModel, UserModel>());
            var mapper = new Mapper(map);

            var claims = await _authService.Register(mapper.Map<UserModel>(userModel));
            await HttpContext.SignInAsync(claims);

            return true;
        }

        [HttpPost("login")]
        public async Task<bool> Login([FromBody] AuthModel authModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidDataException("invalid data found");
            }

            var claims = await _authService.Login(authModel);

            if(!claims.Claims.Any())
            {
                throw new Exception("No such user exists");
            }

            await HttpContext.SignInAsync(claims);

            return true;
        }
    }
}
