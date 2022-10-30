namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkTimesCriteria : BaseStatCriteria
    {
        public decimal AverageWalksCountPerDay { get; set; }

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
            ); //надо протестить

            AverageWalksCountPerDay = (decimal)groupedWalksByDay.Select(x => x.Count).Average();
        }

        protected override CriteriaResult CalculateCriteriaResult(decimal idleWalkTimesPerDay, decimal actualAverageWalkDuring)
        {
            if(actualAverageWalkDuring >= idleWalkTimesPerDay)
            {
                return CriteriaResult.Good;
            }

            return base.CalculateCriteriaResult(idleWalkTimesPerDay, actualAverageWalkDuring);
        }
    }
}
