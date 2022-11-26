using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.Domain;
using PetHelper.ServiceResulting;
using System.Net.Mail;
using System.Security.Claims;
using PetHelper.Domain.Properties;
using PetHelper.Business.User;
using AutoMapper;
using PetHelper.Api.Models.RequestModels;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using PetHelper.Domain.Auth;
using Microsoft.AspNetCore.Http;

namespace PetHelper.Business.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IMapper mapper, IPasswordHasher passwordHasher, IEmailService emailService, 
            IUserService userService, IConfiguration config, IHttpContextAccessor httpContextAccessor) 
        {
            _emailService = emailService;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

            _jwtSettings = config.GetSection("Jwt").Get<JwtSettings>();
        }

        public async Task<ServiceResult<string>> Login(AuthModel authModel)
        {
            var serviceResult = new ServiceResult<string>();

            if (authModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantAuthenticateUser);
            }

            ValidateEmail(authModel.Login, serviceResult);

            var userResult = await _userService.GetUser(x => x.Login == authModel.Login);

            if (!userResult.Value.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow(Resources.EmailConfirmationIsNeeded);
            }

            if (!_passwordHasher.ComparePasswords(authModel.Password, userResult.Value.Password))
            {
                return serviceResult.FailAndThrow(Resources.WrongPasswordOrLogin);
            }

            serviceResult.Value = BuildClaimsWithEmail(userResult.Value);

            return serviceResult.CatchAny();
        }

        public async Task<ServiceResult<string>> Register(UserRequestModel userModel)
        {
            var serviceResult = new ServiceResult<string>();

            if (userModel is null)
            {
                return serviceResult.FailAndThrow(Resources.InvalidDataFoundCantRegisterUser);
            }

            var userDomainModel = _mapper.Map<UserModel>(userModel);

            ValidateEmail(userModel.Login, serviceResult);

            await _userService.AddUser(userDomainModel);
            await _emailService.SendEmailConfirmMessage(userDomainModel);

            serviceResult.Value = BuildInitialClaims(userDomainModel);

            return serviceResult.CatchAny();
        }

        public async Task<ServiceResult<string>> ConfirmEmail(string key)
        {
            var serviceResult = new ServiceResult<string>();

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
            return serviceResult.CatchAny();
        }

        private void ValidateEmail(string login, ServiceResult<string> serviceResult)
        {
            if (!MailAddress.TryCreate(login, out _))
            {
                serviceResult.FailAndThrow(Resources.InvalidEmailAddressFormatSpecified);
            }
        }

        private string BuildInitialClaims(UserModel userModel)
        {
            var claims = ClaimsSets.GetInitialClaims(userModel);
            return BuildClaims(claims);
        }

        private string BuildClaimsWithEmail(UserModel userModel)
        {
            var claims = ClaimsSets.GetClaimsWithEmail(userModel);
            return BuildClaims(claims);
        }

        private string BuildClaims(IEnumerable<Claim> claims)
        {
            var secretBytes = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: signingCreds
            );

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenHandler;
        }

        public async Task<ServiceResult<UserModel>> GetCurrentUser()
        {
            var id = _httpContextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(id == null)
            {
                return new ServiceResult<UserModel>().FailAndThrow("No current user exists");
            }

            var user = await _userService.GetUser(x => x.Id == Guid.Parse(id!));
            return user.CatchAny();
        }
    }
}
