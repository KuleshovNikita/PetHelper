using PetHelper.Business.Hashing;
using PetHelper.DataAccess.Repo;
using PetHelper.Domain;
using System.Globalization;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public class AuthService : DataAccessableService<UserModel>, IAuthService
    {
        public AuthService(IRepository<UserModel> repo) : base(repo) { }

        public ClaimsPrincipal Login(AuthModel authModel)
        {
            if(authModel is null || string.IsNullOrEmpty(authModel.Login) || string.IsNullOrEmpty(authModel.Password))
            {
                throw new InvalidDataException("Invalid data found, can't authenticate user");
            }

            var userModel = _repository.Single(x => x.Login == authModel.Login);

            if(userModel is null)
            {
                return new ClaimsPrincipal();
            }

            var hasher = new PasswordHasher();
            hasher.ComparePasswords(authModel.Password, userModel.Password);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, userModel.Login),
                new Claim(ClaimTypes.Expiration, AfterMinutes(30))
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity); 

            return claimsPrincipal;
        }

        private string AfterMinutes(int lifetime)
            => DateTime.UtcNow.AddMinutes(lifetime).ToString("G", CultureInfo.InvariantCulture);
    }
}
