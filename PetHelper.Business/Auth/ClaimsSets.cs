using Microsoft.IdentityModel.JsonWebTokens;
using PetHelper.Domain;
using System.Security.Claims;

namespace PetHelper.Business.Auth
{
    public static class ClaimsSets
    {
        public static IReadOnlyCollection<Claim> GetInitialClaims(UserModel userModel) 
            => new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString())
            };

        public static IReadOnlyCollection<Claim> GetClaimsWithEmail(UserModel userModel)
            => new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, $"{userModel.FirstName} {userModel.LastName}"),
                new Claim(JwtRegisteredClaimNames.Sub, userModel.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userModel.Login)
            };
    }
}
