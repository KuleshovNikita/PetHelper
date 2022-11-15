using System.Security.Cryptography;

namespace PetHelper.Business.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int RfcIterationsCount = 100000;
        private const int SaltLength = 16;
        private const int HashLength = 20;
        private const int TotalLength = SaltLength + HashLength;

        private readonly RandomNumberGenerator _cryptoProvider = RandomNumberGenerator.Create();

        public string HashPassword(string password)
        {
            var salt = new byte[SaltLength];
            _cryptoProvider.GetBytes(salt);

            var hash = MakeHash(password, salt).GetBytes(HashLength);
            var hashBytes = new byte[TotalLength];

            CopySaltBytes(salt, hashBytes);
            CopyHashBytes(hash, hashBytes);

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
            var hashBytes = Convert.FromBase64String(expectedAsHash);
            var salt = new byte[SaltLength];

            CopySaltBytes(hashBytes, salt);

            return salt;
        }

        private void CopySaltBytes(byte[] from, byte[] to) => Array.Copy(from, 0, to, 0, SaltLength);

        private void CopyHashBytes(byte[] from, byte[] to) => Array.Copy(from, 0, to, SaltLength, HashLength);
    }
}
