using PetHelper.Domain.Pets;
using System.Text.Json.Serialization;

namespace PetHelper.Api.Models.RequestModels.Schedules
{
    public class ScheduleRequestModel
    {
        public TimeSpan ScheduledStart { get; set; }

        public TimeSpan ScheduledEnd { get; set; }

        public Guid PetId { get; set; }

        [JsonIgnore]
        public PetModel Pet { get; set; }
    }
}
