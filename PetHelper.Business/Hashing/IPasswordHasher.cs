namespace PetHelper.Business.Hashing
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool ComparePasswords(string actualAsValue, string expectedAsHash);
    }
}
