using Microsoft.Extensions.DependencyInjection;
using PetHelper.Domain.Modules;

namespace PetHelper.DataAccess.Modules
{
    public class DataAccessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            return new ServiceCollection();
        }
    }
}
