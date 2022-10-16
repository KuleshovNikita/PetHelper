using Microsoft.AspNetCore.Mvc;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [ApiController]
    public abstract class ResultingController : ControllerBase
    {
        protected async Task<ServiceResult<TResult>> RunWithServiceResult<TResult>(Func<Task<ServiceResult<TResult>>> action)
        {
            var result = new ServiceResult<TResult>();

            try
            {
                result = await action();

                return result.Success();
            }
            catch (FailedServiceResultException ex)
            {
                return result.Fail(ex);
            }
        }
    }
}
