using System.Security.Cryptography;

namespace PetHelper.Business.Hashing
{
    public class PasswordHasher
    {
        private const int RfcIterationsCount = 100000;
        private const int SaltLength = 16;
        private const int HashLength = 20;
        private const int TotalLength = SaltLength + HashLength;

        public string HashPassword(string data)
        {
            byte[] salt = new byte[SaltLength];
            new RNGCryptoServiceProvider().GetBytes(salt);

            var derivedBytes = new Rfc2898DeriveBytes(data, salt, RfcIterationsCount);
            byte[] hash = derivedBytes.GetBytes(HashLength);

            byte[] hashBytes = new byte[TotalLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltLength);
            Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public bool ComparePasswords(string actualAsValue, string expectedAsHash)
        {
            var actualHashed = HashPassword(actualAsValue);

            var actualBytes = GetBytesWithoutSalt(actualHashed);
            var expectedBytes = GetBytesWithoutSalt(expectedAsHash);

            for (int i = 0; i < HashLength; i++)
            {
                if (actualBytes[i + SaltLength] != expectedBytes[i])
                {
                    return false;
                }
            }

            return true;                
        }

        private byte[] GetBytesWithoutSalt(string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltLength];
            Array.Copy(hashBytes, 0, salt, 0, SaltLength);
            var pbkdf2 = new Rfc2898DeriveBytes(hashedPassword, salt, RfcIterationsCount);

            byte[] hash = pbkdf2.GetBytes(HashLength);

            return hash;
        }
    }
}
