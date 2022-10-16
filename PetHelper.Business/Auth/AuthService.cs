using Microsoft.AspNetCore.Authentication.Cookies;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Globalization;
using System.Net.Mail;
using System.Security.Claims;
using PetHelper.Domain.Properties;
using PetHelper.Business.User;

namespace PetHelper.Business.Auth
{
    public class AuthService : DataAccessableService<UserModel>, IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IPasswordHasher passwordHasher, IEmailService emailService, 
            IUserService userService, IRepository<UserModel> repo) 
            : base(repo) 
        {
            _emailService = emailService;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Login(AuthModel authModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (authModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantAuthenticateUser);
            }

            ValidateEmail(authModel.Login, serviceResult);

            var userResult = await _userService.GetUser(
                                predicate: x => x.Login == authModel.Login, 
                                messageIfNotFound: Resources.UserWithTheProvidedLoginDoesntExist);

            if(!userResult.Value.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow(Resources.YouCantAuthenticateTheAccountEmailConfirmationIsNeeded);
            }

            if(!_passwordHasher.ComparePasswords(authModel.Password, userResult.Value.Password))
            {
                return serviceResult.FailAndThrow(Resources.WrongPasswordOrLogin);
            }

            serviceResult.Value = BuildClaims(userResult.Value);

            return serviceResult;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Register(UserModel userModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (userModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantRegisterUser);
            }

            ValidateEmail(userModel.Login, serviceResult);

            await _userService.AddUser(userModel);
            await _emailService.SendEmailConfirmMessage(userModel);

            serviceResult.Value = BuildClaims(userModel);

            return serviceResult;
        }

        public async Task<ServiceResult<Empty>> ConfirmEmail(string key)
        {
            var userResult = await _userService.GetUser(
                                predicate: x => x.Password.ToLower() == key.ToLower(), 
                                messageIfNotFound: Resources.NoUsersForSpecifiedKeyWereFound);

            if(userResult.Value.IsEmailConfirmed)
            {
                return new ServiceResult<Empty>().FailAndThrow(Resources.TheUsersEmailIsAlreadyConfirmed);
            }

            userResult.Value.IsEmailConfirmed = true;
            await _userService.UpdateUser(userResult.Value);

            return new ServiceResult<Empty>().Success();
        }

        private void ValidateEmail(string login, ServiceResult<ClaimsPrincipal> serviceResult)
        {
            if (!MailAddress.TryCreate(login, out _))
            {
                serviceResult.FailAndThrow(Resources.InvalidEmailAddressFormatSpecified);
            }
        }

        private ClaimsPrincipal BuildClaims(UserModel userModel)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, userModel.Login),
                new Claim(ClaimTypes.Expiration, AfterMinutes(30))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        private string AfterMinutes(int lifetime)
            => DateTime.UtcNow.AddMinutes(lifetime).ToString("G", CultureInfo.InvariantCulture);
    }
}
