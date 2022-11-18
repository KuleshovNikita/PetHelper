using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Controllers;
using PetHelper.IoT.Business.Auth;
using PetHelper.IoT.Business.Walks;
using PetHelper.IoT.Domain.Auth;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Api.Controllers
{
    public class AuthController : ResultingController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
            => _authService = authService;

        [HttpPost]
        public async Task<ServiceResult<Empty>> Login(AuthModel authModel)
            => await RunWithServiceResult(async () => await _authService.Login(authModel));

    }
}
