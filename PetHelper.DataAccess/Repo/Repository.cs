using Microsoft.EntityFrameworkCore;
using PetHelper.DataAccess.Context;
using PetHelper.Domain.Exceptions;
using System.Linq.Expressions;

namespace PetHelper.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PetHelperDbContext _context;

        public Repository(PetHelperDbContext context) => _context = context;

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> command)
            => await _context.Set<T>().FirstOrDefaultAsync(command) ?? throw new EntityNotFoundException();

        public async Task<bool> Any(Expression<Func<T, bool>> command) 
            => await _context.Set<T>().AnyAsync(command);

        public async Task Insert(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }
    }
}
