using PetHelper.Domain.Extensions;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkDuringCriteria : BaseStatCriteria
    {
        public decimal AverageWalkDuring { get; set; }

        public decimal IdleWalkDuring { get; set; }

        public override void Calculate(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            var actualAverageWalkDuring = walksTime.Average(x => x.EndTime.MinusTime(x.StartTime).ToMinutes());

            AverageWalkDuring = actualAverageWalkDuring;
            CriteriaResult = CalulateCriteriaResult(IdleWalkDuring, actualAverageWalkDuring);
        }
    }
}
