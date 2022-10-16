using System.Security.Cryptography;

namespace PetHelper.Business.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int RfcIterationsCount = 100000;
        private const int SaltLength = 16;
        private const int HashLength = 20;
        private const int TotalLength = SaltLength + HashLength;

        public string HashPassword(string password)
        {
            byte[] salt = new byte[SaltLength];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var hash = MakeHash(password, salt).GetBytes(HashLength);

            byte[] hashBytes = new byte[TotalLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public bool ComparePasswords(string actualAsValue, string expectedAsHash)
        {
            var expectedBytes = Convert.FromBase64String(expectedAsHash);

            var salt = GetSaltOfPassword(expectedAsHash);
            var actualBytes = MakeHash(actualAsValue, salt).GetBytes(HashLength);

            for (int i = 0; i < HashLength; i++)
            {
                if (expectedBytes[i + SaltLength] != actualBytes[i])
                {
                    return false;
                }
            }

            return true;
        }

        private Rfc2898DeriveBytes MakeHash(string password, byte[] salt) 
            => new Rfc2898DeriveBytes(password, salt, RfcIterationsCount);

        private byte[] GetSaltOfPassword(string expectedAsHash)
        {
            byte[] hashBytes = Convert.FromBase64String(expectedAsHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            return salt;
        }
    }
}
