using NUnit.Framework;
using PetHelper.Domain.Statistic.StatisticCriterias;

namespace PetHelper.UnitTests.DomainTests
{
    [TestFixture]
    public class WalkDuringCriteriaTests
    {
        private readonly IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime =
            new List<(DateTime StartTime, DateTime EndTime)>
            {
                (new DateTime(2022, 10, 1, 10, 10, 0), new DateTime(2022, 10, 1, 10, 25, 0)),
                (new DateTime(2022, 10, 1, 10, 30, 0), new DateTime(2022, 10, 1, 10, 40, 0)),
                (new DateTime(2022, 10, 1, 18, 0, 0), new DateTime(2022, 10, 1, 18, 9, 0)),
            }; // avg time is 11.3 minutes

        [TestCase(10, CriteriaResult.Good)]
        [TestCase(11.3, CriteriaResult.Good)]
        [TestCase(13, CriteriaResult.Acceptable)]
        [TestCase(20, CriteriaResult.Bad)]
        [TestCase(40, CriteriaResult.VeryBad)]
        public void CalculatesDataCorrectly(decimal idleValue, CriteriaResult criteriaResult)
        {
            //Arrange 
            var sut = new WalkDuringCriteria();

            //Act
            sut.Calculate(idleValue, walksTime);

            //Assert
            Assert.That(sut.Criteria, Is.EqualTo(criteriaResult));
        }
    }
}
