using Microsoft.AspNetCore.Authentication.Cookies;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Net.Mail;
using System.Security.Claims;
using PetHelper.Domain.Properties;
using PetHelper.Business.User;
using AutoMapper;
using PetHelper.Api.Models.RequestModels;

namespace PetHelper.Business.Auth
{
    public class AuthService : DataAccessableService<UserModel>, IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, IPasswordHasher passwordHasher, IEmailService emailService, 
            IUserService userService, IRepository<UserModel> repo) 
            : base(repo) 
        {
            _emailService = emailService;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Login(AuthModel authModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (authModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantAuthenticateUser);
            }

            ValidateEmail(authModel.Login, serviceResult);

            var userResult = await _userService.GetUser(x => x.Login == authModel.Login);

            if(!userResult.Value.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow(Resources.EmailConfirmationIsNeeded);
            }

            if(!_passwordHasher.ComparePasswords(authModel.Password, userResult.Value.Password))
            {
                return serviceResult.FailAndThrow(Resources.WrongPasswordOrLogin);
            }

            serviceResult.Value = BuildClaimsWithEmail(userResult.Value); //TODO баг, должно делать клаимы с почтой

            return serviceResult;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Register(UserRequestModel userModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (userModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantRegisterUser);
            }

            var userDomainModel = _mapper.Map<UserModel>(userModel);

            ValidateEmail(userModel.Login, serviceResult);

            await _userService.AddUser(userDomainModel);
            await _emailService.SendEmailConfirmMessage(userDomainModel);

            serviceResult.Value = BuildInitialClaims(userDomainModel);

            return serviceResult;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> ConfirmEmail(string key)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            var userResult = await _userService.GetUser(x => x.Password.ToLower() == key.ToLower());
            var userDomainModel = userResult.Value;

            if(userDomainModel.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow(Resources.TheUsersEmailIsAlreadyConfirmed);
            }

            userDomainModel.IsEmailConfirmed = true;
            var userUpdateModel = _mapper.Map<UserUpdateRequestModel>(userDomainModel);

            await _userService.UpdateUser(userUpdateModel, userDomainModel.Id);

            serviceResult.Value = BuildClaimsWithEmail(userDomainModel);
            return serviceResult.Success();
        }

        private void ValidateEmail(string login, ServiceResult<ClaimsPrincipal> serviceResult)
        {
            if (!MailAddress.TryCreate(login, out _))
            {
                serviceResult.FailAndThrow(Resources.InvalidEmailAddressFormatSpecified);
            }
        }

        private ClaimsPrincipal BuildInitialClaims(UserModel userModel)
        {
            var claims = ClaimsSets.GetInitialClaims(userModel);
            return BuildClaims(claims);
        }

        private ClaimsPrincipal BuildClaimsWithEmail(UserModel userModel)
        {
            var claims = ClaimsSets.GetClaimsWithEmail(userModel);
            return BuildClaims(claims);
        }

        private ClaimsPrincipal BuildClaims(IEnumerable<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}
