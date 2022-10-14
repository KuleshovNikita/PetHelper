using Microsoft.Extensions.DependencyInjection;

namespace PetHelper.Domain.Modules
{
    public interface IModule
    {
        IServiceCollection ConfigureModule(IServiceCollection services);
    }
}
