using System.Text.Json.Serialization;

namespace PetHelper.Domain.Pets
{
    public record PetModel : BaseModel
    {
        public string Name { get; set; } = null!;

        public AnimalType? AnimalType { get; set; }

        public string? Breed { get; set; }

        public double AllowedDistance { get; set; }

        public bool IsWalking { get; set; }

        public IEnumerable<ScheduleModel> WalkingSchedule { get; set; } = null!;

        public IEnumerable<WalkModel> WalksHistory { get; set; } = null!;

        public Guid OwnerId { get; set; }

        [JsonIgnore]
        public UserModel Owner { get; set; } = null!;
    }
}
