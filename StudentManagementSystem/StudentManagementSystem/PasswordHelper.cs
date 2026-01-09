using System;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagementSystem
{
    public static class PasswordHelper
    {
        // Create hash and salt
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512()) // generates random salt
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        // Verify password
        public static bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return CryptographicOperations.FixedTimeEquals(computedHash, storedHash);
            }
        }
    }
}
