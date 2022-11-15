using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public interface IWalkRepository
    {
        Task<ServiceResult<IEnumerable<WalkModel>>> Where(Expression<Func<WalkModel, bool>> command);
    }
}
