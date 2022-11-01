using AutoMapper;
using Moq;
using NUnit.Framework;
using PetHelper.Api.Models.RequestModels.Pets;
using PetHelper.Business.Extensions;
using PetHelper.Business.Pet;
using PetHelper.Business.User;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.Domain.Exceptions;
using PetHelper.Domain.Pets;
using PetHelper.ServiceResulting;
using PetHelper.UnitTests.Utils;

namespace PetHelper.UnitTests.BusinessTests
{
    [TestFixture]
    public class PetServiceTests : TestBase<PetService>
    {
        private Mock<IMapper> _mockMapper;
        private Mock<IUserService> _mockUserService;
        private Mock<IRepository<PetModel>> _mockRepository;

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserService = new Mock<IUserService>();
            _mockRepository = new Mock<IRepository<PetModel>>();
        }

        [Test]
        public void AddPet_ThrowsForInvalidModel()
        {
            //Arrange
            var MockRandomGuid = Guid.NewGuid();
            var sut = GetNewSut();

            //Act & Assert
            InvokeActAndAssertFailedResult(() => sut.AddPet(null!, MockRandomGuid));
        }

        [Test]
        public void AddPet_ThrowsForInvalidGuid()
        {
            //Arrange
            var sut = GetNewSut();

            //Act & Assert
            InvokeActAndAssertFailedResult(() => sut.AddPet(new PetRequestModel(), Guid.Empty));
        }

        [Test]
        public void AddPet_AddsPetSuccessFully()
        {
            //Arrange
            var petRequestModel = new PetRequestModel();
            var userId = Guid.NewGuid();

            var userModel = new UserModel
            {
                Id = userId,
                IsEmailConfirmed = true
            };

            var petDomainModel = new PetModel();

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(userModel));

            _mockMapper
                .Setup(x => x.Map<PetModel>(petRequestModel))
                .Returns(petDomainModel);

            _mockRepository
                .Setup(x => x.Insert(petDomainModel))
                .Returns(TaskServiceResult<Empty>(null!));

            var sut = GetNewSut();

            AsyncTestDelegate actionExceptions = () => sut.AddPet(petRequestModel, userId);

            //Act
            var actionResult = () => sut.AddPet(petRequestModel, userId);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.DoesNotThrowAsync(actionExceptions);
                Assert.That(actionResult().Result.IsSuccessful);
            });
        }

        [Test]
        public void GetPet_GetsPetCorrectly()
        {
            //Arrange
            _mockRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<PetModel>()))
                .Returns(TaskServiceResult(Any<PetModel>()));

            var sut = GetNewSut();

            //Act
            AsyncTestDelegate action = () => sut.GetPet(AnyExpression<PetModel>());

            //Assert
            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public void GetPet_ThrowsIfNoPetFound()
        {
            //Arrange
            var repoResult = new ServiceResult<PetModel>().Fail(new EntityNotFoundException());

            _mockRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<PetModel>()))
                .Returns(Task.FromResult(repoResult));

            var sut = GetNewSut();

            //Act
            AsyncTestDelegate action = () => sut.GetPet(AnyExpression<PetModel>());

            //Assert
            Assert.ThrowsAsync<FailedServiceResultException>(action);
        }

        [Test]
        public void RemovePet_RemovesPetCorrectly()
        {
            //Arrange
            _mockRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<PetModel>()))
                .Returns(TaskServiceResult(Any<PetModel>()));

            _mockRepository
                .Setup(x => x.Any(AnyExpression<PetModel>()))
                .Returns(TaskServiceResult(true));

            _mockRepository
                .Setup(x => x.Remove(Any<PetModel>()))
                .Returns(TaskServiceResult(Any<Empty>()));

            var sut = GetNewSut();

            //Act
            AsyncTestDelegate action = () => sut.RemovePet(Guid.NewGuid());

            //Assert
            Assert.DoesNotThrowAsync(action);
        }

        [Test]
        public void RemovePet_ThrowsIfNoPetExists()
        {
            //Arrange
            _mockRepository
                .Setup(x => x.FirstOrDefault(AnyExpression<PetModel>()))
                .Returns(TaskServiceResult(Any<PetModel>()));

            _mockRepository
                .Setup(x => x.Any(AnyExpression<PetModel>()))
                .Returns(TaskServiceResult(false));

            _mockRepository
                .Setup(x => x.Remove(Any<PetModel>()))
                .Returns(TaskServiceResult(Any<Empty>()));

            var sut = GetNewSut();

            //Act
            AsyncTestDelegate action = () => sut.RemovePet(Guid.NewGuid());

            //Assert
            Assert.ThrowsAsync<FailedServiceResultException>(action);
        }

        protected override PetService GetNewSut()
            => new PetService(_mockUserService.Object,
                              _mockMapper.Object,
                              _mockRepository.Object);
    }
}
