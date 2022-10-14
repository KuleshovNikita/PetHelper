using PetHelper.DataAccess.Context;

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
    }
}
