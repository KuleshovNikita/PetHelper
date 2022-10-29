using PetHelper.Domain.Extensions;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkDuringCriteria : BaseStatCriteria
    {
        public decimal AverageWalkDuring { get; set; }

        public override void Calculate(decimal idleValue, IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            var actualAverageWalkDuring = walksTime.Average(x => x.EndTime.MinusTime(x.StartTime).ToMinutes());

            AverageWalkDuring = actualAverageWalkDuring;
            CriteriaResult = CalulateCriteriaResult(idleValue, actualAverageWalkDuring);
        }
    }
}
