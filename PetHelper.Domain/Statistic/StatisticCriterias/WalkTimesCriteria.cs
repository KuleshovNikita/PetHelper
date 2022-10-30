namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkTimesCriteria : BaseStatCriteria
    {
        public decimal AverageWalksCountPerDay { get; set; }

        protected override IDictionary<int, CriteriaResult> CriteriaResultOrder => new Dictionary<int, CriteriaResult>
        {
            [0] = CriteriaResult.Good,
            [1] = CriteriaResult.Acceptable,
            [2] = CriteriaResult.Bad,
            [3] = CriteriaResult.VeryBad,
        };

        public override void Calculate(decimal IdleValue, IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            CalculateCommon(walksTime);
            Criteria = CalculateCriteriaResult(IdleValue, AverageWalksCountPerDay);
        }

        public override void CalculateWithoutIdle(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            CalculateCommon(walksTime);
            Criteria = null;
        }

        private void CalculateCommon(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
        {
            var groupedWalksByDay = walksTime.GroupBy(
                x => x.StartTime.Date,
                x => walksTime.Where(y => y.StartTime.Date == x.StartTime.Date),
                (key, count) => new
                {
                    Key = key,
                    Count = count.Count(),
                }
            );

            AverageWalksCountPerDay = (decimal)groupedWalksByDay.Select(x => x.Count).Average();
        }

        protected override CriteriaResult CalculateCriteriaResult(decimal idleValue, decimal actualAverageData)
        {
            if (actualAverageData >= idleValue)
            {
                return CriteriaResult.Good;
            }

            var skippedWalksCount = (int)(idleValue - actualAverageData);

            if(skippedWalksCount > (int)Enum.GetValues<CriteriaResult>().Max())
            {
                return CriteriaResult.VeryBad;
            }

            return CriteriaResultOrder[skippedWalksCount];
        }
    }
}
