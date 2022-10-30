using PetHelper.Api.Models.RequestModels.Schedules;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using System.Linq.Expressions;

namespace PetHelper.Business.Schedule
{
    public interface IScheduleService
    {
        Task<ServiceResult<Empty>> AddSchedule(ScheduleRequestModel scheduleRequestModel);

        Task<ServiceResult<ScheduleModel>> GetSchedule(Expression<Func<ScheduleModel, bool>> predicate);

        Task<ServiceResult<IEnumerable<ScheduleModel>>> GetSchedules(Expression<Func<ScheduleModel, bool>> predicate);

        Task<ServiceResult<Empty>> UpdateSchedule(ScheduleUpdateRequestModel scheduleUpdateModel, Guid scheduleId);

        Task<ServiceResult<Empty>> RemoveSchedule(Guid scheduleId);
    }
}
