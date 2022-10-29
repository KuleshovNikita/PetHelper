using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.Business.Extensions;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic;
using PetHelper.Domain.Statistic.StatisticCriterias;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Statistic
{
    public class StatisticService : DataAccessableService<WalkModel>, IStatisticService
    {
        public StatisticService(IRepository<WalkModel> repo) : base(repo) { }

        public async Task<ServiceResult<StatisticModel>> GetStatistic(StatisticRequestModel model)
        {
            var walkResult = await _repository.Where(x => x.StartTime == model.SampleStartDate
                                                  && x.EndTime == model.SampleEndDate
                                                  && x.PetId == model.PetId);  

            var walksData = walkResult.CatchAny().Value;

            var statistic = new StatisticModel();
            //TODO создать таблицу с идеальными значениями статистики для каждого животного и достать их оттуда

            var walksTimeHistory = walksData.Select(x => (StartTime: x.StartTime, EndTime: x.EndTime));

            statistic.WalkDuringCriteria.Calculate(statistic.IdlePetStatisticModel.IdleWalkDuringTime, walksTimeHistory);
            statistic.WalksCountCriteria.Calculate(statistic.IdlePetStatisticModel.IdleWalksCountPerDay, walksTimeHistory);
        }
    }
}
