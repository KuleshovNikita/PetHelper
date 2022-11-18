namespace PetHelper.IoT.Domain.Auth
{
    public class AuthModel
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ReturnUrl { get; set; }
    }
}
