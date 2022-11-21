using Microsoft.Extensions.DependencyInjection;
using PetHelper.IoT.Domain.Modules;
using PetHelper.IoT.ServerClient.Auth;
using PetHelper.IoT.ServerClient.PetsClient;
using PetHelper.IoT.ServerClient.WalksClient;

namespace PetHelper.IoT.ServerClient.Modules
{
    public class ServerClientModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped<IPetClient, PetClient>();
            services.AddScoped<IAuthClient, AuthClient>();
            services.AddScoped<IWalkClient, WalkClient>();

            services.AddSingleton<ServerClient>();

            return services;
        }
    }
}
