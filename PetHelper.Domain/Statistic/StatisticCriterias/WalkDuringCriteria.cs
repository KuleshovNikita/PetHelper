using PetHelper.Domain.Extensions;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkDuringCriteria : BaseStatCriteria
    {
        public decimal AverageWalkDuring { get; set; }

        public override void Calculate(decimal idleValue, IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            CalculateCommon(walksTime);
            Criteria = CalculateCriteriaResult(idleValue, AverageWalkDuring);
        }

        public override void CalculateWithoutIdle(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            CalculateCommon(walksTime);
            Criteria = null;
        }

        private void CalculateCommon(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
            => AverageWalkDuring = walksTime.Average(x => x.EndTime.MinusTime(x.StartTime).ToMinutes());
    }
}
