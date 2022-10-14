using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Auth;
using PetHelper.Domain;

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
        public async Task<bool> Register([FromBody] UserRequestModel userModel)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidDataException("invalid data found");
            }

            var claims = await _authService.Register(_mapper.Map<UserModel>(userModel));
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
