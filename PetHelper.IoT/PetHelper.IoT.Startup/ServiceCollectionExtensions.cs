using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PetHelper.IoT.Business.Modules;
using PetHelper.IoT.DeviceController.Modules;
using PetHelper.IoT.Domain.Modules;
using PetHelper.IoT.Domain.PetModels;
using PetHelper.IoT.Domain.WalksModels;
using PetHelper.IoT.ServerClient.Modules;

namespace PetHelper.IoT.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddModule<BusinessModule>();
            services.AddModule<DeviceControllerModule>();
            services.AddModule<ServerClientModule>();

            services.AddAutoMapper(GetAutoMapperConfigs());
        }

        private static IServiceCollection AddModule<TModule>(this IServiceCollection services)
            where TModule : IModule, new()
        {
            var module = new TModule();
            module.ConfigureModule(services);

            return services;
        }

        private static Action<IMapperConfigurationExpression> GetAutoMapperConfigs()
            => cfg =>
            {
                cfg.CreateMap<PetModel, WalkOptions>();
            };
    }
}