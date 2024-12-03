using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Domain.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _iterations = 10000;  // Iteration sayısı
        private readonly int _saltLength = 16;     // Salt uzunluğu
        private readonly int _hashLength = 32;     // Hash uzunluğu

        public string HashPassword(string plainTextPassword)
        {
            
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[_saltLength];
            rng.GetBytes(salt);

            using var hashAlgorithm = new Rfc2898DeriveBytes(plainTextPassword, salt, _iterations);
            byte[] hash = hashAlgorithm.GetBytes(_hashLength);

            byte[] hashBytes = new byte[_saltLength + _hashLength];
            Array.Copy(salt, 0, hashBytes, 0, _saltLength);
            Array.Copy(hash, 0, hashBytes, _saltLength, _hashLength);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string plainTextPassword, string storedHash)
        {
            // Saklanan hash ve salt'i çözümle
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[_saltLength];
            Array.Copy(hashBytes, 0, salt, 0, _saltLength);

            // Kullanıcıdan alınan şifre ile aynı hash'i oluştur
            using var hashAlgorithm = new Rfc2898DeriveBytes(plainTextPassword, salt, _iterations);
            byte[] hash = hashAlgorithm.GetBytes(_hashLength);

            // Hesaplanan hash ile saklanan hash'i karşılaştır
            for (int i = 0; i < _hashLength; i++)
            {
                if (hashBytes[i + _saltLength] != hash[i])
                    return false;
            }

            return true;
        }
    }

}
