using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace TimejApi.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }

    /// <summary>
    /// Create simple hash using Pbkdf2. 
    /// </summary>
    /// <remarks>
    /// Results are encoded into base64
    /// </remarks>
    public class PasswordHasher : IPasswordHasher
    {
        public const int SaltSize = 16;
        public const int HashSize = 20;

        public PasswordHasher()
        {
        }

        public string HashPassword(string password, int saltSize, int hashSize)
        {
            byte[] salt = new byte[saltSize];
            RandomNumberGenerator.Create().GetBytes(salt);
            byte[] hash = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, hashSize);
            byte[] result = new byte[saltSize + hashSize];
            Array.Copy(salt, result, saltSize);
            Array.Copy(hash, 0, result, saltSize, hashSize);

            return Convert.ToBase64String(result);
        }

        public string HashPassword(string password) => HashPassword(password, SaltSize, HashSize);

        public bool VerifyPassword(string password, string passwordHash, int saltSize, int hashSize)
        {
            var salt = new byte[saltSize];
            var hashedData = Convert.FromBase64String(passwordHash);
            Array.Copy(hashedData, salt, saltSize);
            var hashToCheck = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 10000, hashSize);
            for (int i = 0; i < hashSize; ++i)
            {
                if (hashedData[i + saltSize] != hashToCheck[i]) return false;
            }
            return true;
        }

        public bool VerifyPassword(string password, string passwordHash) => VerifyPassword(password, passwordHash, SaltSize, HashSize);
    }
}
