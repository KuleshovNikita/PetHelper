using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Controllers;
using PetHelper.IoT.DeviceController.Controllers.Sounds;
using PetHelper.IoT.Domain.Sounds;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.Api.Controllers
{
    public class SoundsController : ResultingController
    {
        private readonly ISoundController _soundController;

        public SoundsController(ISoundController soundController)
        {
            _soundController = soundController;
        }

        [HttpGet]
        public async Task<ServiceResult<IEnumerable<SoundModel>>> GetSounds()
            => await RunWithServiceResult(async () => await _soundController.GetSounds());
    }
}
