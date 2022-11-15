using Microsoft.Extensions.DependencyInjection;
using PetHelper.IoT.Business.Modules;
using PetHelper.IoT.Domain.Modules;

namespace PetHelper.IoT.Startup
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddModule<BusinessModule>();
        }

        private static IServiceCollection AddModule<TModule>(this IServiceCollection services)
            where TModule : IModule, new()
        {
            var module = new TModule();
            module.ConfigureModule(services);

            return services;
        }
    }
}