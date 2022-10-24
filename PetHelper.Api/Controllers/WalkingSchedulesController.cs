using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Business.Schedule;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;

namespace PetHelper.Api.Controllers
{
    [Authorize]
    public class WalkingSchedulesController : ResultingController
    {
        private readonly IScheduleService _scheduleService;

        public WalkingSchedulesController(IScheduleService scheduleService) => _scheduleService = scheduleService;

        [HttpPost("addSchedule")]
        public async Task<ServiceResult<Empty>> AddSchedule([FromBody] ScheduleRequestModel scheduleModel)
            => await RunWithServiceResult(async () => await _scheduleService.AddSchedule(scheduleModel));

        [HttpPut("updateSchedule/{scheduleId:guid}")]
        public async Task<ServiceResult<Empty>> UpdateSchedule(Guid scheduleId, [FromBody] ScheduleUpdateRequestModel scheduleModel)
            => await RunWithServiceResult(async () => await _scheduleService.UpdateSchedule(scheduleModel, scheduleId));

        [HttpGet("getSchedule/{scheduleId:guid}")]
        public async Task<ServiceResult<ScheduleModel>> GetSchedule(Guid scheduleId)
            => await RunWithServiceResult(async () => await _scheduleService.GetSchedule(x => x.Id == scheduleId));

        [HttpGet("getPetSchedules/{petId:guid}")]
        public async Task<ServiceResult<IEnumerable<ScheduleModel>>> GetScheduleForPet(Guid petId)
            => await RunWithServiceResult(async () => await _scheduleService.GetSchedules(x => x.PetId == petId));

        [HttpDelete("removeSchedule/{scheduleId:guid}")]
        public async Task<ServiceResult<Empty>> RemoveSchedule(Guid scheduleId)
            => await RunWithServiceResult(async () => await _scheduleService.RemoveSchedule(scheduleId));
    }
}
