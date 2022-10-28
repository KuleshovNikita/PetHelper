using PetHelper.Domain.Pets;
using System.Text.Json.Serialization;

namespace PetHelper.Api.Models.RequestModels.Schedules
{
    public class ScheduleRequestModel
    {
        public DateTime ScheduledStart { get; set; }

        public DateTime ScheduledEnd { get; set; }

        public Guid PetId { get; set; }

        [JsonIgnore]
        public PetModel Pet { get; set; }
    }
}
