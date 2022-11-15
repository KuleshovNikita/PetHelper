using Microsoft.AspNetCore.Cors.Infrastructure;

namespace PetHelper.IoT.Api.Extensions
{
    public static class CorsOptionsExtensions
    {
        public static void AllowAnyOriginPolicy(this CorsOptions source)
            => source.AddPolicy(
                    name: "AllowOrigin",
                    configurePolicy: p => p.AllowAnyOrigin()
                                           .AllowAnyHeader()
                                           .AllowCredentials()
                                           .AllowAnyMethod()
            );
    }
}
