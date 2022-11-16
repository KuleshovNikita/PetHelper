namespace PetHelper.Domain.Auth
{
    public record JwtSettings
    {
        public string Issuer { get; init; } = null!;

        public string Audience { get; init; } = null!;

        public string Secret { get; init; } = null!;

        public int ExpiresInMinutes { get; init; } = 0;
    }
}
