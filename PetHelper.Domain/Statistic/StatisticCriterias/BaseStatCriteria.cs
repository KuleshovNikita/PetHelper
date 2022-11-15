namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public abstract record BaseStatCriteria
    {
        public CriteriaResult? Criteria { get; set; }

        protected abstract IDictionary<int, CriteriaResult> CriteriaResultOrder { get; }

        public abstract void Calculate(decimal idleValue, IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime);

        public abstract void CalculateWithoutIdle(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime);

        protected abstract CriteriaResult CalculateCriteriaResult(decimal idleValue, decimal actualAverageData);
    }
}
