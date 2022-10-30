using PetHelper.Domain.Statistic.StatisticCriterias;

namespace PetHelper.UnitTests.DomainTests
{
    [TestFixture]
    public class WalkTimeCriteriaTests
    {
        private readonly IEnumerable<(DateTime StartTime, DateTime EndTime)> walksTime =
            new List<(DateTime StartTime, DateTime EndTime)>
            {
                (new DateTime(2022, 10, 1, 10, 10, 0), new DateTime(2022, 10, 1, 10, 25, 0)),
                (new DateTime(2022, 10, 1, 15, 30, 0), new DateTime(2022, 10, 1, 10, 40, 0)),
                (new DateTime(2022, 10, 1, 18, 0, 0), new DateTime(2022, 10, 1, 18, 9, 0)),
            };

        //[TestCase(2, CriteriaResult.Good)]
        //[TestCase(3, CriteriaResult.Good)]
        [TestCase(4, CriteriaResult.Acceptable)]
        [TestCase(5, CriteriaResult.Bad)]
        [TestCase(6, CriteriaResult.VeryBad)]
        [TestCase(100, CriteriaResult.VeryBad)]
        public void CalculatesDataCorrectly(decimal idleValue, CriteriaResult criteriaResult)
        {
            //Arrange 
            var sut = new WalkTimesCriteria();

            //Act
            sut.Calculate(idleValue, walksTime);

            //Assert
            Assert.That(sut.Criteria, Is.EqualTo(criteriaResult));
        }
    }
}
