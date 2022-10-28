using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Statistic
{
    public class StatisticService : DataAccessableService<PetModel>, IStatisticService
    {
        public StatisticService(IRepository<PetModel> repo) : base(repo) { }

        public Task<ServiceResult<StatisticModel>> GetStatistic(StatisticRequestModel model)
        {
            throw new NotImplementedException();
        }
    }
}
