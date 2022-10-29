namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public record WalkTimesCriteria : BaseStatCriteria
    {
        public decimal AverageWalkTimesPerDay { get; set; }

        public decimal IdleWalkTimesPerDay { get; set; }

        public override void Calculate(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime)
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

            var actualAverageWalkTimesPerDay = (decimal)groupedWalksByDay.Select(x => x.Count).Average();

            AverageWalkTimesPerDay = actualAverageWalkTimesPerDay;
            CriteriaResult = CalulateCriteriaResult(IdleWalkTimesPerDay, actualAverageWalkTimesPerDay);
        }

        protected override CriteriaResult CalulateCriteriaResult(decimal idleWalkTimesPerDay, decimal actualAverageWalkDuring)
        {
            if(actualAverageWalkDuring >= idleWalkTimesPerDay)
            {
                return CriteriaResult.Good;
            }

            return base.CalulateCriteriaResult(idleWalkTimesPerDay, actualAverageWalkDuring);
        }
    }
}
