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
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, saltSize + hashSize));
        }

        public string HashPassword(string password) => HashPassword(password, SaltSize, HashSize);

        public bool VerifyPassword(string password, string passwordHash, int saltSize, int hashSize)
        {
            var salt = new byte[saltSize];
            Array.Copy(Convert.FromBase64String(passwordHash), salt, saltSize);
            return password == Convert.ToBase64String(
                KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA256, 100000, saltSize + hashSize));
        }

        public bool VerifyPassword(string password, string passwordHash) => VerifyPassword(password, passwordHash, SaltSize, HashSize);
    }
}
