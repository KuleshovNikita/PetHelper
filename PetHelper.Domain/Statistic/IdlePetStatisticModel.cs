using PetHelper.Domain.Pets;

namespace PetHelper.Domain.Statistic
{
    public record IdlePetStatisticModel : BaseModel
    {
        public AnimalType AnimalType { get; set; }

        public string Breed { get; set; } = null!;

        public decimal IdleWalkDuringTime { get; set; }

        public decimal IdleWalksCountPerDay { get; set; }
    }
}
