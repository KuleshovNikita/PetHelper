using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.Domain.Statistic;
using PetHelper.ServiceResulting;

namespace PetHelper.Business.Statistic
{
    public interface IStatisticService
    {
        Task<ServiceResult<StatisticModel>> GetStatistic(StatisticRequestModel model);
    }
}
