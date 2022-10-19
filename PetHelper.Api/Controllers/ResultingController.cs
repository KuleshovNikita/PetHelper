using Microsoft.AspNetCore.Mvc;
using PetHelper.ServiceResulting;
using System.Security.Claims;

namespace PetHelper.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        protected ServiceResult<Empty> SuccessEmptyResult() => new ServiceResult<Empty>().Success();

        protected Guid GetUserIdFromToken()
        {
            var userId = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return new Guid(userId);
        }
    }
}
