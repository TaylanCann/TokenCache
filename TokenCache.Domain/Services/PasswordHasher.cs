using System.Security.Cryptography;
using TokenCache.Domain.Entities;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Domain.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly int _iterations = 10000;  // Iteration sayısı
        private readonly int _saltLength = 16;     // Salt uzunluğu
        private readonly int _hashLength = 32;     // Hash uzunluğu
        private readonly IWordRepository _wordRepository;
        private readonly IUserRepository _userRepository;

        public PasswordHasher(IWordRepository wordRepository, IUserRepository userRepository)
        {
            _wordRepository = wordRepository;
            _userRepository = userRepository;
        }

        public string HashPassword(User user, string plainTextPassword)
        {
            //Bu kullanıcı için türetilmiş bir kelime var mı yoksa kullanıcı kayıt mı oluyor kontrol edilecek.
            await checkUserExist(user);

            ArgumentNullException.ThrowIfNull(user);           
            ArgumentNullException.ThrowIfNull(plainTextPassword);

            var checkUserRegister = _userRepository.UserExistsAsync(user.Username);

            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[_saltLength];
            rng.GetBytes(salt);

            using var hashAlgorithm = new Rfc2898DeriveBytes(plainTextPassword, salt, _iterations);
            byte[] hash = hashAlgorithm.GetBytes(_hashLength);

            byte[] hashBytes = new byte[_saltLength + _hashLength];
            Array.Copy(salt, 0, hashBytes, 0, _saltLength);
            Array.Copy(hash, 0, hashBytes, _saltLength, _hashLength);

            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }
        public string CreateWord()
        {
            string word = string.Empty;
           
            var rnd = new Random();
            for (int i = 0; i< 6; i++)
            {
                word += ((char)rnd.Next('A', 'Z')).ToString();
            }
            
            return word;

        }

        private async bool checkUserExist(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            var checkUserRegister = _userRepository.UserExistsAsync(user.Username);

            return await checkUserRegister;
        }

    }


}
