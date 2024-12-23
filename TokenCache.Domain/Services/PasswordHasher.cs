using System;
using System.Security.Cryptography;
using System.Text;
using TokenCache.Domain.Entities;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Domain.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        
        private readonly IWordRepository _wordRepository;
        private readonly IUserRepository _userRepository;
        private readonly Random _random = new();

        public PasswordHasher(IWordRepository wordRepository, IUserRepository userRepository)
        {
            _wordRepository = wordRepository;
            _userRepository = userRepository;
        }

        public async Task<string> HashPassword(string username,string Password)
        {
            //Bu kullanıcı için türetilmiş bir kelime var mı yoksa kullanıcı kayıt mı oluyor kontrol edilecek.
            ArgumentNullException.ThrowIfNull(Password);

            var saltedPassword = await SaltPassword(username,Password);

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(Password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
        public async Task<string> SaltPassword(string username, string Password)
        {
            string word = string.Empty;
           
            var rnd = new Random();
            var rndCount = new Random();
            rndCount.Next(5, 10).ToString();
            int.Parse(rndCount);

            for (int i = 0; i< rndCount; i++)
            {
                word += ((char)rnd.Next('A', 'Z')).ToString();
            }



            await _wordRepository.CreateAsync(new Word { UserId = username, WordText = word });

            return word;

        }

        

    }


}
