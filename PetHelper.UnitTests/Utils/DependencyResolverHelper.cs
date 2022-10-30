using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace PetHelper.UnitTests.Utils
{
    public class DependencyResolverHelper
    {
        private readonly IWebHost _webHost;

        public DependencyResolverHelper()
        {
            _webHost = WebHost.CreateDefaultBuilder()
                              .UseStartup<Program>()
                              .Build();
        }

        public T GetService<T>()
        {
            var services = _webHost.Services;

            try
            {
                var scopedService = (T)services.GetService(typeof(T))!;
                return scopedService;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
