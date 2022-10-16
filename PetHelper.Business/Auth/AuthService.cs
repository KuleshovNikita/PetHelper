﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetHelper.Business.Email;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using PetHelper.Domain.Exceptions;
using PetHelper.ServiceResulting;
using System.Globalization;
using System.Net.Mail;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public class AuthService : DataAccessableService<UserModel>, IAuthService
    {
        private readonly IEmailService _emailService;
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public AuthService(IEmailService emailService, IRepository<UserModel> repo) : base(repo) 
        {
            _emailService = emailService;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Login(AuthModel authModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (authModel is null)
            {
                return serviceResult.FailAndThrow("Invalid data found, can't authenticate user");
            }

            ValidateEmail(authModel.Login, serviceResult);

            var userResult = 
                (await _repository.FirstOrDefault(x => x.Login == authModel.Login))
                    .Catch<EntityNotFoundException>("User with the provided login doesn't exist")
                    .Catch<ArgumentNullException>()
                    .Catch<InvalidOperationException>()
                    .Catch<OperationCanceledException>();

            if(!userResult.Value.IsEmailConfirmed)
            {
                return serviceResult.FailAndThrow("You can't authenticate the account, email confirmation is needed");
            }

            if(!_passwordHasher.ComparePasswords(authModel.Password, userResult.Value.Password))
            {
                return serviceResult.FailAndThrow("Wrong password or login");
            }

            serviceResult.Value = BuildClaims(userResult.Value);

            return serviceResult;
        }

        public async Task<ServiceResult<ClaimsPrincipal>> Register(UserModel userModel)
        {
            var serviceResult = new ServiceResult<ClaimsPrincipal>();

            if (userModel is null)
            {
                return serviceResult.FailAndThrow("Invalid data found, can't register user");
            }

            ValidateEmail(userModel.Login, serviceResult);

            if(await LoginIsAlreadyRegistered(userModel.Login))
            {
                return serviceResult.FailAndThrow("The login is already registered");
            }

            var hashedPassword = _passwordHasher.HashPassword(userModel.Password);
            userModel.Password = hashedPassword;
            userModel.Id = Guid.NewGuid();

            (await _repository.Insert(userModel))
                    .Catch<OperationCanceledException>()
                    .Catch<DbUpdateException>()
                    .Catch<DbUpdateConcurrencyException>(); 

            await _emailService.SendEmailConfirmMessage(userModel);

            serviceResult.Value = BuildClaims(userModel);

            return serviceResult;
        }

        public async Task<ServiceResult<Empty>> ConfirmEmail(string key)
        {
            var userResult = 
                (await _repository.FirstOrDefault(x => x.Password.ToLower() == key.ToLower()))
                    .Catch<ArgumentNullException>()
                    .Catch<OperationCanceledException>()
                    .Catch<EntityNotFoundException>("No users for specified key were found");

            if(userResult.Value.IsEmailConfirmed)
            {
                return new ServiceResult<Empty>().FailAndThrow("The user's email is already confirmed");
            }

            userResult.Value.IsEmailConfirmed = true;

            (await _repository.Update(userResult.Value))
                .Catch<OperationCanceledException>()
                .Catch<DbUpdateException>()
                .Catch<DbUpdateConcurrencyException>();

            return new ServiceResult<Empty>().Success();
        }

        private void ValidateEmail(string login, ServiceResult<ClaimsPrincipal> serviceResult)
        {
            if (!MailAddress.TryCreate(login, out _))
            {
                serviceResult.FailAndThrow("Invalid email address format specified");
            }
        }

        private async Task<bool> LoginIsAlreadyRegistered(string login)
        {
            var serviceResult = new ServiceResult<bool>();

            serviceResult = 
                (await _repository.Any(x => x.Login == login))
                    .Catch<OperationCanceledException>()
                    .Catch<ArgumentNullException>();

            return serviceResult.Value;
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
