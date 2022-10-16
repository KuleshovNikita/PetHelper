using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Domain.Exceptions;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PetHelperDbContext _context;

        public Repository(PetHelperDbContext context) => _context = context;

        public async Task<ServiceResult<T>> FirstOrDefault(Expression<Func<T, bool>> command)
        {
            var result = new ServiceResult<T>();

            try
            {
                result.Value = await _context.Set<T>().FirstOrDefaultAsync(command) ?? throw new EntityNotFoundException();

                return result.Success();
            }
            catch (Exception ex)
            {
                return result.Fail(ex);
            }
        }

        public async Task<ServiceResult<bool>> Any(Expression<Func<T, bool>> command)
        {
            var result = new ServiceResult<bool>();

            try
            {
                result.Value = await _context.Set<T>().AnyAsync(command);

                return result.Success();
            }
            catch(Exception ex)
            {
                return result.Fail(ex);
            }
        }

        public async Task<ServiceResult<Empty>> Insert(T entity)
        {
            var result = new ServiceResult<Empty>();

            try
            {
                await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return result.Success();
            }
            catch(Exception ex)
            {
                return result.Fail(ex);
            }
        }

        public Task<ServiceResult<Empty>> Update(T entity)
        {
            var result = new ServiceResult<Empty>();

            try
            {
                _context.Set<T>().Update(entity);
                _context.SaveChangesAsync();

                return Task.FromResult(result.Success());
            }
            catch (Exception ex)
            {
                return Task.FromResult(result.Fail(ex));
            }
        }
    }
}
