using Microsoft.Extensions.Configuration;
using PetHelper.IoT.Domain.Sounds;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.DeviceController.Controllers.Sounds
{
    public class SoundController : ISoundController
    {
        private readonly string _storagePath;

        public SoundController(IConfiguration config)
        {
            _storagePath = config.GetSection("StoragePath").Value!;
            _storagePath = $"{_storagePath}\\Sounds";
        }

        public Task<ServiceResult<IEnumerable<SoundModel>>> GetSounds()
        {
            var serviceResult = new ServiceResult<IEnumerable<SoundModel>>();

            try
            {
                var soundsDir = Directory.CreateDirectory(_storagePath);
                var soundsList = soundsDir.GetFiles("*.mp3");
                serviceResult.Value = soundsList.Select(x => new SoundModel { Name = x.Name });

                return Task.FromResult(serviceResult.Success());
            }
            catch (Exception ex)
            {
                return Task.FromResult(serviceResult.FailAndThrow(ex.Message));
            }
        }
    }
}
