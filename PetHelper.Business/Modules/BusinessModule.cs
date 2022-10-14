using Microsoft.Extensions.DependencyInjection;
using PetHelper.Domain.Modules;

namespace PetHelper.Business.Modules
{
    public class BusinessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            return new ServiceCollection();
        }
    }
}
