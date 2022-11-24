using PetHelper.IoT.Domain.Sounds;
using PetHelper.ServiceResulting;

namespace PetHelper.IoT.DeviceController.Controllers.Sounds
{
    public interface ISoundController
    {
        Task<ServiceResult<IEnumerable<SoundModel>>> GetSounds();
    }
}
