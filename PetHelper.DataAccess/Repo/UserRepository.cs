using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Domain;
using PetHelper.Domain.Exceptions;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        public UserRepository(PetHelperDbContext context) : base(context)
        {
        }

        public override async Task<ServiceResult<UserModel>> FirstOrDefault(Expression<Func<UserModel, bool>> command)
        {
            var result = new ServiceResult<UserModel>();

            try
            {
                result.Value = await _context.Set<UserModel>().Include(u => u.Pets)
                                        .FirstOrDefaultAsync(command) ?? throw new EntityNotFoundException();

                return result.Success();
            }
            catch (Exception ex)
            {
                return result.Fail(ex);
            }
        }
    }
}
