using PetHelper.DataAccess.Repo;

namespace PetHelper.Business
{
    public abstract class DataAccessableService<T> where T : class
    {
        protected readonly IRepository<T> _repository;

        public DataAccessableService(IRepository<T> repository)
        {
            _repository = repository;
        }
    }
}
