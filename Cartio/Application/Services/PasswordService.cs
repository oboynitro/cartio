using Cartio.Application.Abstractions.Services;
using System;

namespace Cartio.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string ComputeSalt(int size = 64)
        {
            Random rnd = new Random();
            byte[] b = new byte[size];
            rnd.NextBytes(b);
            return Convert.ToBase64String(b);
        }

        public (string hash, string salt) HashPassword(string rawPassword)
        {
            string salt = ComputeSalt();
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(string.Concat(rawPassword, salt));
            return (passwordHash, salt);
        }

        public bool VerifyPassword(string rawPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(rawPassword, hashedPassword);
        }
    }
}
