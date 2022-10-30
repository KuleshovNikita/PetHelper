using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Business.Schedule;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Authorize]
    public class ScheduleController : ResultingController
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService) => _scheduleService = scheduleService;

        [HttpPost("")]
        public async Task<ServiceResult<Empty>> AddSchedule([FromBody] ScheduleRequestModel scheduleModel)
            => await RunWithServiceResult(async () => await _scheduleService.AddSchedule(scheduleModel));

        [HttpPut("{scheduleId:guid}")]
        public async Task<ServiceResult<Empty>> UpdateSchedule(Guid scheduleId, [FromBody] ScheduleUpdateRequestModel scheduleModel)
            => await RunWithServiceResult(async () => await _scheduleService.UpdateSchedule(scheduleModel, scheduleId));

        [HttpGet("{scheduleId:guid}")]
        public async Task<ServiceResult<ScheduleModel>> GetSchedule(Guid scheduleId)
            => await RunWithServiceResult(async () => await _scheduleService.GetSchedule(x => x.Id == scheduleId));

        [HttpGet("pet/{petId:guid}")]
        public async Task<ServiceResult<IEnumerable<ScheduleModel>>> GetScheduleForPet(Guid petId)
            => await RunWithServiceResult(async () => await _scheduleService.GetSchedules(x => x.PetId == petId));

        [HttpDelete("{scheduleId:guid}")]
        public async Task<ServiceResult<Empty>> RemoveSchedule(Guid scheduleId)
            => await RunWithServiceResult(async () => await _scheduleService.RemoveSchedule(scheduleId));
    }
}
