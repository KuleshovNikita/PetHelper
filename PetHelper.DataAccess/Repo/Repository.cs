using PetHelper.DataAccess.Context;
using PetHelper.Domain;

namespace PetHelper.DataAccess.Repo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly PetHelperDbContext _context;

        public Repository(PetHelperDbContext context)
        {
            _context = context;
        }

        public T Single(Func<T, bool> command)
        {
            var user = _context.Set<T>().Single(command);

            return user;
        }

        public bool Any(Func<T, bool> command) => _context.Set<T>().Any(command);

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }
    }
}
