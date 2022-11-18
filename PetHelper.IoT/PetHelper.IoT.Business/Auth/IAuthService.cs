using PetHelper.IoT.Domain.Auth;
using PetHelper.ServiceResulting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetHelper.IoT.Business.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<Empty>> Login(AuthModel authModel);
    }
}
