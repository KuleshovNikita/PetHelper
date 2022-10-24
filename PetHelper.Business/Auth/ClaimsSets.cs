using PetHelper.Domain;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public static class ClaimsSets
    {
        public static IReadOnlyCollection<Claim> GetInitialClaims(UserModel userModel) 
            => new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString())
            };

        public static IReadOnlyCollection<Claim> GetClaimsWithEmail(UserModel userModel)
            => new List<Claim>
            {
                new Claim(ClaimTypes.Name, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                new Claim(ClaimTypes.Email, userModel.Login)
            };
    }
}
