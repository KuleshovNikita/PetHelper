using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public class WalkRepository : Repository<WalkModel>, IWalkRepository
    {
        public WalkRepository(PetHelperDbContext context) : base(context)
        {
            
        }

        public override Task<ServiceResult<IEnumerable<WalkModel>>> Where(Expression<Func<WalkModel, bool>> command)
        {
            var result = new ServiceResult<IEnumerable<WalkModel>>();

            try
            {
                result.Value = _context.Set<WalkModel>()
                                        .Include(x => x.Pet)
                                        .Where(command) ?? throw new EntityNotFoundException();

                return Task.FromResult(result.Success());
            }
            catch (Exception ex)
            {
                return Task.FromResult(result.Fail(ex));
            }
        }
    }
}
