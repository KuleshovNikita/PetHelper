using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.Business.Statistic;
using PetHelper.Domain.Statistic;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Authorize]
    public class StatisticController : ResultingController
    {
        private readonly IStatisticService _statisticService;

        public StatisticController(IStatisticService statisticService) => _statisticService = statisticService;

        [HttpPost("")]
        public async Task<ServiceResult<StatisticModel>> GetPet([FromBody] StatisticRequestModel statisticModel)
            => await RunWithServiceResult(async () => await _statisticService.GetStatistic(statisticModel));
    }
}
