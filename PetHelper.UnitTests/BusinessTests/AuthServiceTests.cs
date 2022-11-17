using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using PetHelper.Api.Models.RequestModels;
using PetHelper.Business.Auth;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.Business.User;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using PetHelper.UnitTests.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PetHelper.UnitTests.BusinessTests
{
    [TestFixture]
    public class AuthServiceTests : TestBase<AuthService>
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IEmailService> _mockEmailService;
        private Mock<IPasswordHasher> _mockPasswordHasher;
        private Mock<IMapper> _mockMapper;
        private Mock<IConfiguration> _mockConfig;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockEmailService = new Mock<IEmailService>();
            _mockPasswordHasher = new Mock<IPasswordHasher>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();
        }

        #region LoginTests

        [Test]
        public void Login_Throws_IfModel_Null()
        {
            //Arrange
            AuthModel authModel = null!;
            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Login(authModel));
        }

        [Test]
        public void Login_Throws_ForInvalidEmailFormat()
        {
            //Arrange
            var authModel = new AuthModel
            {
                Login = "invalid email"
            };

            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Login(authModel));
        }

        [Test]
        public void Login_Throws_ForUnconfirmedEmail()
        {
            //Arrange
            var userEmail = "valid.email@mail.com";

            var authModel = new AuthModel
            {
                Login = userEmail
            };

            var user = new UserModel
            {
                Login = userEmail,
                IsEmailConfirmed = false
            };

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(user));

            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Login(authModel));
        }

        [Test]
        public void Login_Throws_ForWrongPassword()
        {
            //Arrange
            var userEmail = "valid.email@mail.com";

            var authModel = new AuthModel
            {
                Login = userEmail,
                Password = "wrong password"
            };

            var user = new UserModel
            {
                Login = userEmail,
                IsEmailConfirmed = true,
                Password = "correct password"
            };

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(user));

            _mockPasswordHasher
                .Setup(x => x.ComparePasswords(Any<string>(), Any<string>()))
                .Returns(false);

            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Login(authModel));
        }

        [Test]
        public void Login_BuildsClaimsWithEmail()
        {
            //Arrange
            var userEmail = "valid.email@mail.com";
            var userPassword = "password";

            var authModel = new AuthModel
            {
                Login = userEmail,
                Password = userPassword
            };

            var user = new UserModel
            {
                FirstName = "first name",
                LastName = "last name",
                Id = Guid.NewGuid(),
                Login = userEmail,
                IsEmailConfirmed = true,
                Password = userPassword
            };

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(user));

            _mockPasswordHasher
                .Setup(x => x.ComparePasswords(Any<string>(), Any<string>()))
                .Returns(true);

            var sut = GetNewSut();

            //Act 
            var result = sut.Login(authModel).Result;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful);
                Assert.That(result.Value is not null);
            });
        }

        #endregion
        #region RegisterTests

        [Test]
        public void Register_ThrowsIfModelNull()
        {
            //Arrange
            UserRequestModel requestModel = null!;
            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Register(requestModel));
        }

        [Test]
        public void Register_ThrowsForInvalidEmailFormat()
        {
            //Arrange
            var requestModel = new UserRequestModel
            {
                Login = "invalid email"
            };

            var sut = GetNewSut();

            //Act & Arrange
            InvokeActAndAssertFailedResult(() => sut.Register(requestModel));
        }

        [Test]
        public void Register_BuildsClaimsWithoutEmail()
        {
            //Arrange
            var userEmail = "valid.email@mail.com";

            var userRequestModel = new UserRequestModel
            {
                Login = userEmail
            };

            var user = new UserModel
            {
                FirstName = "first name",
                LastName = "last name",
                Id = Guid.NewGuid(),
                Login = userEmail,
                IsEmailConfirmed = true,
                Password = "any",
            };

            _mockMapper
                .Setup(x => x.Map<UserModel>(Any<UserRequestModel>()))
                .Returns(user);

            _mockUserService
                .Setup(x => x.AddUser(user))
                .Returns(TaskServiceResult<Empty>(null!));

            _mockEmailService
                .Setup(x => x.SendEmailConfirmMessage(user))
                .Returns(TaskServiceResult<Empty>(null!));

            var sut = GetNewSut();

            //Act 
            var result = sut.Register(userRequestModel).Result;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful);
                Assert.That(result.Value is not null);
            });
        }

        #endregion
        #region ConfirmEmailTests

        [Test]
        public void ConfirmEmail_Throws_IfEmailIsAlreadyConfirmed()
        {
            //Arrange
            var userModel = new UserModel
            {
                IsEmailConfirmed = true
            };

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(userModel));

            var sut = GetNewSut();

            //Act & Assert
            InvokeActAndAssertFailedResult(() => sut.ConfirmEmail(Any<string>()));
        }

        [Test]
        public void ConfirmEmail_BuildsClaimsWithEmail()
        {
            //Arrange
            var userModel = new UserModel
            {
                IsEmailConfirmed = false,
                Id = Guid.NewGuid(),
                FirstName = "first name",
                LastName = "last name",
                Login = "valid.email@mail.com",
                Password = "any"
            };

            var userRequestModel = new UserUpdateRequestModel
            {
                Id = Guid.NewGuid(),
                FirstName = "first name",
                LastName = "last name",
                Login = "valid.email@mail.com",
                Password = "any"
            };

            _mockUserService
                .Setup(x => x.GetUser(AnyExpression<UserModel>()))
                .Returns(TaskServiceResult(userModel));

            _mockUserService
                .Setup(x => x.UpdateUser(userRequestModel, Any<Guid>()))
                .Returns(TaskServiceResult<Empty>(null!));

            _mockMapper
                .Setup(x => x.Map<UserUpdateRequestModel>(userModel))
                .Returns(userRequestModel);

            var sut = GetNewSut();

            //Act
            var result = sut.ConfirmEmail(Any<string>()).Result;

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccessful);
                Assert.That(result.Value is not null);
            });
        }

        #endregion

        protected override AuthService GetNewSut()
            => new AuthService(_mockMapper.Object,
                     _mockPasswordHasher.Object,
                     _mockEmailService.Object,
                     _mockUserService.Object, 
                     _mockConfig.Object);
    }
}
