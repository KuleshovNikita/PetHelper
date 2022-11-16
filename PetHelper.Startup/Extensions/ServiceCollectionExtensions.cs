using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.Business.Modules;
using PetHelper.DataAccess.Context;
using PetHelper.DataAccess.Modules;
using PetHelper.Domain;
using PetHelper.Domain.Auth;
using PetHelper.Domain.Modules;
using PetHelper.Domain.Pets;
using System.Text;
using System.Text.Json;

namespace PetHelper.Startup.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddModule<BusinessModule>();
            services.AddModule<DataAccessModule>();

            services.AddAutoMapper(GetAutoMapperConfigs());

            return services;
        }

        public static IServiceCollection ConfigureDbConnection(this IServiceCollection services, ConfigurationManager config)
            => services
                .AddDbContext<PetHelperDbContext>(
                    opt => opt.UseSqlServer(config.GetConnectionString("SqlServer"))
                );

        public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
            => services
                .AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", cfg => 
                {
                    var secretBytes = Encoding.UTF8.GetBytes(jwtSettings.Secret);
                    var key = new SymmetricSecurityKey(secretBytes);

                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = key
                    };
                });

        private static void SetSimpleAuthentication(this CookieAuthenticationOptions opt,
            ConfigurationManager config)
        {
            var expireTime = config.GetSection("TokenExpirationTime").Value;
            var cookieName = config.GetSection("CookieTokenName").Value;

            opt.Cookie.Name = cookieName;
            opt.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(expireTime));
            opt.SlidingExpiration = true;
            opt.Events.OnRedirectToLogin = (op) => Task.FromResult(op.Response.StatusCode = 401);
        }

        private static IServiceCollection AddModule<TModule>(this IServiceCollection services)
            where TModule : IModule, new()
        {
            var module = new TModule();
            module.ConfigureModule(services);

            return services;
        }

        private static Action<IMapperConfigurationExpression> GetAutoMapperConfigs()
            => cfg =>
            {
                cfg.CreateMap<UserRequestModel, UserModel>();
                cfg.CreateMap<UserUpdateRequestModel, UserModel>();
                cfg.CreateMap<UserModel, UserUpdateRequestModel>();

                cfg.CreateMap<PetRequestModel, PetModel>();


                cfg.CreateMap<ScheduleRequestModel, ScheduleModel>();
                cfg.CreateMap<ScheduleUpdateRequestModel, ScheduleModel>();

                cfg.CreateMap<WalkRequestModel, WalkModel>();
            };
    }
}
