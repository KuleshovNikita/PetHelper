using Microsoft.Extensions.DependencyInjection;
using PetHelper.Business.Auth;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.Business.Pet;
using PetHelper.Business.Schedule;
using PetHelper.Business.Statistic;
using PetHelper.Business.User;
using PetHelper.Business.Walk;
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
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddScoped<IWalkService, WalkService>();

            return services;
        }
    }
}
