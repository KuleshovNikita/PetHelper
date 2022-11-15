using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelper.IoT.Domain.Modules
{
    public interface IModule
    {
        public IServiceCollection ConfigureModule(IServiceCollection services);
    }
}
