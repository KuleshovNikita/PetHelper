﻿using Microsoft.AspNetCore.Authentication.Cookies;
using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using System.Globalization;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public class AuthService : DataAccessableService<UserModel>, IAuthService
    {
        private readonly PasswordHasher _passwordHasher = new PasswordHasher();

        public AuthService(IRepository<UserModel> repo) : base(repo) { }

        public async Task<ClaimsPrincipal> Login(AuthModel authModel)
        {
            if(authModel is null || string.IsNullOrEmpty(authModel.Login) || string.IsNullOrEmpty(authModel.Password))
            {
                throw new InvalidDataException("Invalid data found, can't authenticate user");
            }

            var userModel = await _repository.Single(x => x.Login == authModel.Login);

            if(userModel is null)
            {
                return new ClaimsPrincipal();
            }

            if(!_passwordHasher.ComparePasswords(authModel.Password, userModel.Password))
            {
                throw new Exception("Wrong password");
            }

            return BuildClaims(userModel);
        }

        public async Task<ClaimsPrincipal> Register(UserModel userModel)
        {
            if(userModel is null || string.IsNullOrEmpty(userModel.FirstName) || 
                string.IsNullOrEmpty(userModel.LastName) || string.IsNullOrEmpty(userModel.Login) ||
                string.IsNullOrEmpty(userModel.Password))
            {
                throw new InvalidDataException("Invalid data found, can't register user");
            }

            if(await _repository.Any(x => x.Login == userModel.Login))
            {
                throw new Exception("User with such email is already registered");
            }

            var hashedPassword = _passwordHasher.HashPassword(userModel.Password);

            userModel.Password = hashedPassword;
            userModel.Id = Guid.NewGuid();

            await _repository.Insert(userModel);

            return BuildClaims(userModel);
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
