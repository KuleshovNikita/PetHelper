using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public class PetRepository : Repository<PetModel>, IPetRepository
    {
        public PetRepository(PetHelperDbContext context) : base(context)
        {
        }

        public override async Task<ServiceResult<PetModel>> FirstOrDefault(Expression<Func<PetModel, bool>> command)
        {
            var result = new ServiceResult<PetModel>();

            try
            {
                result.Value = await _context.Set<PetModel>().Include(p => p.WalkingSchedule)
                                        .FirstOrDefaultAsync(command) ?? throw new EntityNotFoundException();

                return result.Success();
            }
            catch (Exception ex)
            {
                return result.Fail(ex);
            }
        }

        public override Task<ServiceResult<IEnumerable<PetModel>>> Where(Expression<Func<PetModel, bool>> command)
        {
            var result = new ServiceResult<IEnumerable<PetModel>>();

            try
            {
                result.Value = _context.Set<PetModel>().Include(p => p.WalkingSchedule)
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
