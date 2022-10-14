using Microsoft.AspNetCore.Mvc;
using PetHelper.Domain;

namespace PetHelper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost("login")]
        public void Login([FromBody] AuthModel authModel)
        {
            //TODO выполнить запрос к БД на аунтификацию, если успешно - перенаправить по returnUrl
        }
    }
}
