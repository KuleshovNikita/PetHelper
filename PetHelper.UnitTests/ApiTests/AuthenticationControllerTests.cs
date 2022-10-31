using PetHelper.Api.Controllers;
using PetHelper.Business.Auth;
using PetHelper.UnitTests.Utils;

namespace PetHelper.UnitTests.ApiTests
{
    //[TestFixture]
    public class AuthenticationControllerTests
    {
        private readonly DependencyResolverHelper _dependencyHelper = new DependencyResolverHelper();
        private readonly AuthenticationController _sut;

        private IAuthService _authService;

        [SetUp]
        public void Setup()
        {
            _authService = _dependencyHelper.GetService<IAuthService>();
        }

        //[Test]
        public void Register_RegisterNewUserSuccessfully()
        {

        }
    }
}
