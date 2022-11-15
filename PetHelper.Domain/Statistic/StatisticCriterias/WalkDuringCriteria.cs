using PetHelper.Domain.Extensions;

namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkDuringCriteria : BaseStatCriteria
    {
        public decimal AverageWalkDuring { get; set; }

        protected override IDictionary<int, CriteriaResult> CriteriaResultOrder => new Dictionary<int, CriteriaResult>
        {
            [1] = CriteriaResult.VeryBad,
            [2] = CriteriaResult.Bad,
            [3] = CriteriaResult.Acceptable,
            [4] = CriteriaResult.Good
        };

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

        protected override CriteriaResult CalculateCriteriaResult(decimal idleValue, decimal actualAverageData)
        {
            if (actualAverageData >= idleValue)
            {
                return CriteriaResult.Good;
            }

            var criteriaTypesCount = Enum.GetValues<CriteriaResult>().Count();
            var criteriaStepSize = idleValue / criteriaTypesCount;

            var actualCriteriaScore = (int)Math.Round(actualAverageData / criteriaStepSize, MidpointRounding.AwayFromZero);

            if(actualCriteriaScore < 1)
            {
                return CriteriaResult.VeryBad;
            }

            return CriteriaResultOrder[actualCriteriaScore];
        }
    }
}
