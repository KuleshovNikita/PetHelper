using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.Business.Modules;
using PetHelper.DataAccess.Modules;
using PetHelper.Domain;
using PetHelper.Domain.Modules;
using PetHelper.Domain.Pets;

namespace PetHelper.Startup.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddModule<BusinessModule>();
            services.AddModule<DataAccessModule>();

            services.AddAutoMapper(GetAutoMapperConfigs());

            return services;
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
