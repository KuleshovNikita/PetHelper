namespace PetHelper.Api.Models.RequestModels.Schedules
{
    public class ScheduleUpdateRequestModel
    {
        public TimeSpan? ScheduledStart { get; set; }

        public TimeSpan? ScheduledEnd { get; set; }
    }
}
