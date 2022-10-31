using Microsoft.AspNetCore.Authentication.Cookies;

namespace PetHelper.Api.Extensions
{
    public static class CookieAuthenticationOptionsExtensions
    {
        public static void SetSimpleAuthentication(this CookieAuthenticationOptions opt, 
            WebApplicationBuilder builder)
        {
            var expireTime = builder.Configuration.GetSection("TokenExpirationTime").Value;
            var cookieName = builder.Configuration.GetSection("CookieTokenName").Value;

            opt.Cookie.Name = cookieName;
            opt.ExpireTimeSpan = TimeSpan.FromMinutes(int.Parse(expireTime));
            opt.SlidingExpiration = true;
            opt.Events.OnRedirectToLogin = (op) => Task.FromResult(op.Response.StatusCode = 401);
        }
    }
}
