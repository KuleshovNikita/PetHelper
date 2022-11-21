using Microsoft.Extensions.DependencyInjection;
using PetHelper.IoT.DeviceController.Controllers.Sounds;
using PetHelper.IoT.DeviceController.Controllers.Storage;
using PetHelper.IoT.Domain.Modules;

namespace PetHelper.IoT.DeviceController.Modules
{
    public class DeviceControllerModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped<IStorageController, StorageController>();
            services.AddScoped<ISoundController, SoundController>();

            return services;
        }
    }
}
