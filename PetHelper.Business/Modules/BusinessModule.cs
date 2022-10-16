using Microsoft.Extensions.DependencyInjection;
using PetHelper.Business.Auth;
using PetHelper.Business.Email;
using PetHelper.Domain.Modules;

namespace PetHelper.Business.Modules
{
    public class BusinessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
