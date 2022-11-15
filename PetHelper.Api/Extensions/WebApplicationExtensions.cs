using System.Globalization;

namespace PetHelper.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void ConfigureCulture(this WebApplication app)
        {
            var culture = app.Configuration.GetSection("Culture").Value;

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }
    }
}
