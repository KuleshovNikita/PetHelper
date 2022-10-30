namespace PetHelper.Api.Models.RequestModels.Schedules
{
    public class ScheduleUpdateRequestModel
    {
        public DateTime? ScheduledStart { get; set; }

        public DateTime? ScheduledEnd { get; set; }
    }
}
