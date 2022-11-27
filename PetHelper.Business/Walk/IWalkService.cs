using PetHelper.Api.Models.RequestModels.Walks;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Walk
{
    public interface IWalkService
    {
        Task<ServiceResult<WalkModel>> StartWalk(WalkRequestModel walkRequestModel);

        Task<ServiceResult<WalkModel>> FinishWalk(Guid walkId);

        Task<ServiceResult<IEnumerable<WalkModel>>> GetWalks(Expression<Func<WalkModel, bool>> predicate);
    }
}
