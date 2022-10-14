using PetHelper.Domain;

namespace PetHelper.DataAccess.Repo
{
    public interface IRepository<T> where T : class
    {
        T Single(Func<T, bool> command);

        bool Any(Func<T, bool> command);

        void Insert(T entity);
    }
}
