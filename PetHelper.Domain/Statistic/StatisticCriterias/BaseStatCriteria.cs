namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public abstract record BaseStatCriteria
    {
        public CriteriaResult CriteriaResult { get; set; }

        public abstract void Calculate(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime);

        protected virtual CriteriaResult CalulateCriteriaResult(decimal idleValue, decimal actualAverageWalkDuring)
        {
            var criteriaTypesCount = Enum.GetValues<CriteriaResult>().Count();
            var criteriaStepSize = idleValue / criteriaTypesCount;

            var actualCriteriaScore = Math.Round(actualAverageWalkDuring / criteriaStepSize, MidpointRounding.AwayFromZero);
            return (CriteriaResult)actualCriteriaScore;
        }
    }
}
