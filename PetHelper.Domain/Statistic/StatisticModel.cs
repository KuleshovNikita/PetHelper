using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic.StatisticCriterias;

namespace PetHelper.Domain.Statistic
{
    public record StatisticModel
    {
        public DateTime SampleStartDate { get; set; }

        public DateTime SampleEndDate { get; set; }

        public IdlePetStatisticModel IdlePetStatisticModel { get; set; } = null!;

        public WalkDuringCriteria WalkDuringCriteria { get; set; } = null!;

        public WalkTimesCriteria WalksCountCriteria { get; set; } = null!;

        public CriteriaResult? GeneralResult { get; set; } = null!;

        public PetModel Pet { get; set; } = null!;
    }
}
