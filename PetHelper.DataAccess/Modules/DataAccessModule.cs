using Microsoft.Extensions.DependencyInjection;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Modules;

namespace PetHelper.DataAccess.Modules
{
    public class DataAccessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IWalkRepository, WalkRepository>();

            return services;
        }
    }
}
