using Moq;
using PetHelper.Api.Models.RequestModels.Statistic;
using PetHelper.Business.Statistic;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.Domain.Statistic;
using PetHelper.Domain.Statistic.StatisticCriterias;
using PetHelper.ServiceResulting;
using PetHelper.UnitTests.MockStorages;
using PetHelper.UnitTests.Utils;
using System.Linq.Expressions;

namespace PetHelper.UnitTests.BusinessTests
{
    public class StatisticServiceTests
    {
        private Mock<IRepository<IdlePetStatisticModel>> _mockIdleStaticRepository;
        private Mock<IWalkRepository> _mockWalkRepository;

        [SetUp]
        public void Setup()
        {
            _mockIdleStaticRepository = new Mock<IRepository<IdlePetStatisticModel>>();
            _mockWalkRepository = new Mock<IWalkRepository>();
        }

        [Test]
        public void GetStatistic_CalculatesGeneralStatistic_WhenNoAnimalTypeSpecified()
        {
            //Arrange
            var targetPetId = Guid.Parse("20000000-0000-0000-0000-000000000001");
            var mockStatisticData = MockPetStatistic(targetPetId);
            var sut = GetNewSut();

            //Act
            var result = sut.GetStatistic(mockStatisticData).Result;

            //Assert
            Assert.True(result.IsSuccessful);
            Assert.That(result.Value.WalkDuringCriteria.AverageWalkDuring, Is.EqualTo((decimal)12.5));
            Assert.That(result.Value.WalksCountCriteria.AverageWalksCountPerDay, Is.EqualTo(2));
        }

        [Test]
        public void GetStatistic_CalculatesStatistic_WithCriteriaResult()
        {
            //Arrange
            var targetPetId = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var targetPet = PetMockStorage.Data.First(x => x.Id == targetPetId);
            var mockStatisticData = MockPetStatistic(targetPetId);

            _mockIdleStaticRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<IdlePetStatisticModel>()))
                .Returns(TaskServiceResult(MockIdleDataStorage.Data.First(x => x.Breed == targetPet.Breed
                                                                            && x.AnimalType == targetPet.AnimalType)));

            var sut = GetNewSut();

            //Act
            var result = sut.GetStatistic(mockStatisticData).Result;

            //Assert
            Assert.True(result.IsSuccessful);
            Assert.That(result.Value.WalkDuringCriteria.AverageWalkDuring, Is.EqualTo((decimal)16.5));
            Assert.That(result.Value.WalksCountCriteria.AverageWalksCountPerDay, Is.EqualTo(2));
        }

        [Test]
        public void GetStatistic_CalculatesGeneralStatistic_WhenBreedNull()
        {
            //Arrange
            var targetPetId = Guid.Parse("20000000-0000-0000-0000-000000000003");
            var targetPet = PetMockStorage.Data.First(x => x.Id == targetPetId);
            var mockStatisticData = MockPetStatistic(targetPetId);

            _mockIdleStaticRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<IdlePetStatisticModel>()))
                .Returns(TaskServiceResult(MockIdleDataStorage.Data.First(x => x.AnimalType == targetPet.AnimalType
                                                                            && x.IsUnifiedAnimalData == true)));

            var sut = GetNewSut();

            //Act
            var result = sut.GetStatistic(mockStatisticData).Result;

            //Assert
            Assert.True(result.IsSuccessful);
            Assert.That(result.Value.WalkDuringCriteria.AverageWalkDuring, Is.EqualTo((decimal)16.5));
            Assert.That(result.Value.WalksCountCriteria.AverageWalksCountPerDay, Is.EqualTo(2));
        }

        [Test]
        public void GetStatistic_CalculatesGeneralStatistic_WhenNoIdleDataInDb()
        {
            //Arrange
            var targetPetId = Guid.Parse("20000000-0000-0000-0000-000000000002");
            var targetPet = PetMockStorage.Data.First(x => x.Id == targetPetId);
            var mockStatisticData = MockPetStatistic(targetPetId);

            _mockIdleStaticRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<IdlePetStatisticModel>()))
                .Returns(TaskServiceResult(MockIdleDataStorage.Data.First(x => x.AnimalType == targetPet.AnimalType
                                                                            && x.IsUnifiedAnimalData == true)));

            _mockIdleStaticRepository
                .Setup(x => x.FirstOrDefault(x => x.Breed == targetPet.Breed && x.AnimalType == targetPet.AnimalType))
                .Returns(TaskServiceResultFail<IdlePetStatisticModel>());

            var sut = GetNewSut();

            //Act
            var result = sut.GetStatistic(mockStatisticData).Result;

            //Assert
            Assert.True(result.IsSuccessful);
            Assert.That(result.Value.WalkDuringCriteria.AverageWalkDuring, Is.EqualTo((decimal)16.5));
            Assert.That(result.Value.WalksCountCriteria.AverageWalksCountPerDay, Is.EqualTo(2));
        }

        private StatisticRequestModel MockPetStatistic(Guid targetPetId)
        {
            var statisticRequestModel = new StatisticRequestModel
            {
                PetId = targetPetId,
                SampleStartDate = new DateTime(2022, 11, 1),
                SampleEndDate = new DateTime(2022, 12, 1)
            };

            _mockWalkRepository = new Mock<IWalkRepository>();
            _mockWalkRepository
                .Setup(x => x.Where( AnyExpression<WalkModel>() ))
                .Returns(TaskServiceResult(MockWalkStorage.Data.Where(x => x.PetId == targetPetId)));

            return statisticRequestModel;
        }

        private StatisticService GetNewSut()
            => new StatisticService(_mockWalkRepository.Object, _mockIdleStaticRepository.Object);

        private Expression<Func<T, bool>> AnyExpression<T>() => It.IsAny<Expression<Func<T, bool>>>();

        private Task<ServiceResult<T>> TaskServiceResult<T>(T value)
            => Task.FromResult(new ServiceResult<T> { Value = value }.Success());

        private Task<ServiceResult<T>> TaskServiceResultFail<T>()
            => Task.FromResult(new ServiceResult<T>().Fail(new EntityNotFoundException()));
    }
}
