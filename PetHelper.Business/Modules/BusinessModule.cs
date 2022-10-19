using Microsoft.Extensions.DependencyInjection;
using PetHelper.Business.Auth;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.Business.Pet;
using PetHelper.Business.Schedule;
using PetHelper.Business.User;
using PetHelper.Domain.Modules;

namespace PetHelper.Business.Modules
{
    public class BusinessModule : IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IScheduleService, ScheduleService>();

            return services;
        }
    }
}
