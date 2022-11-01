using NUnit.Framework;
using PetHelper.Business.Hashing;
using PetHelper.UnitTests.Utils;

namespace PetHelper.UnitTests.BusinessTests
{
    [TestFixture]
    public class PasswordHasherTests : TestBase<PasswordHasher>
    {
        [Test]
        public void HashPassword_ChangesPasswordString()
        {
            //Arrange
            var sut = GetNewSut();
            var password = "password";

            //Act
            var hashedPassword = sut.HashPassword(password);

            //Arrange
            Assert.That(hashedPassword, Is.Not.EqualTo(password));
        }

        [Test]
        public void HashPassword_GeneratesDifferentHashesForSamePassword()
        {
            //Arrange
            var sut = GetNewSut();
            var password = "password";
            var passwordGenerationsCount = 10;
            var hashCollention = new string[passwordGenerationsCount];

            //Act
            for(int i = 0; i < 10; i++)
            {
                hashCollention[i] = sut.HashPassword(password);
            }

            //Arrange
            Assert.That(hashCollention.Distinct().Count(), Is.EqualTo(passwordGenerationsCount));
        }

        [Test]
        public void ComparePasswords_ComparesPasswordsCorrectly()
        {
            //Arrange 
            var sut = GetNewSut();
            var password = "password";
            var hashedPassword = sut.HashPassword(password);

            //Act
            var passwordsMatch = sut.ComparePasswords(password, hashedPassword);

            //Assert
            Assert.IsTrue(passwordsMatch);
        }

        [Test]
        public void ComparePasswords_FailsForDiferentPasswords()
        {
            //Arrange 
            var sut = GetNewSut();
            var actualPassword = "actualPassword";
            var expectedPassword = "expectedPassword";
            var hashedExpectedPassword = sut.HashPassword(expectedPassword);

            //Act
            var passwordsMatch = sut.ComparePasswords(actualPassword, hashedExpectedPassword);

            //Assert
            Assert.IsFalse(passwordsMatch);
        }

        protected override PasswordHasher GetNewSut()
            => new PasswordHasher();
    }
}
