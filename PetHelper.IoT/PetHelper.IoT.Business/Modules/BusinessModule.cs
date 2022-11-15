using Microsoft.Extensions.DependencyInjection;
using PetHelper.IoT.Business.Walks;
using PetHelper.IoT.Domain.Modules;

namespace PetHelper.IoT.Business.Modules
{
    public class BusinessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped<IWalkService, WalkService>();

            return services;
        }
    }
}
