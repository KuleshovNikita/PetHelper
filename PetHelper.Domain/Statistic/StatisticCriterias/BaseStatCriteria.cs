﻿namespace PetHelper.Domain.Statistic.StatisticCriterias
{
    public abstract record BaseStatCriteria
    {
        public CriteriaResult? Criteria { get; set; }

        public abstract void Calculate(decimal idleValue, IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime);

        public abstract void CalculateWithoutIdle(IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime);

        protected virtual CriteriaResult CalculateCriteriaResult(decimal idleValue, decimal actualAverageWalkDuring)
        {
            var criteriaTypesCount = Enum.GetValues<CriteriaResult>().Count();
            var criteriaStepSize = idleValue / criteriaTypesCount;

            var actualCriteriaScore = Math.Round(actualAverageWalkDuring / criteriaStepSize, MidpointRounding.AwayFromZero);
            return (CriteriaResult)actualCriteriaScore;
        }
    }
}
