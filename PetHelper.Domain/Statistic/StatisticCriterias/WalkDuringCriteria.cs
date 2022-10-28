namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkDuringCriteria : BaseStatCriteria
    {
        public double AverageWalkDuring { get; set; }

        public double IdleWalkDuring { get; set; }
    }
}
