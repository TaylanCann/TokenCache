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

        public PasswordHasher(IWordRepository wordRepository, IUserRepository userRepository)
        {
            _wordRepository = wordRepository;
            _userRepository = userRepository;
        }

        public async Task<string> HashPassword(string password)
        {
            ArgumentNullException.ThrowIfNull(password);
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        //public async Task<string> HashPasswordLogin(User user)
        //{
        //    var word = await _wordRepository.GetWordById(user.Id);

        //    //Bu kullanıcı için türetilmiş bir kelime var mı yoksa kullanıcı kayıt mı oluyor kontrol edilecek.
        //    ArgumentNullException.ThrowIfNull(user.Password);

        //    var saltedPassword = await SaltPassword(user.Username, user.Password);

        //    using var sha256 = SHA256.Create();
        //    var bytes = Encoding.UTF8.GetBytes(user.Password);
        //    var hash = sha256.ComputeHash(bytes);
        //    return Convert.ToBase64String(hash);
        //}

        //public async Task<string> SaltPassword(string username, string Password)
        //{
        //    string word = string.Empty;
           
        //    var rnd = new Random();
        //    var rndCount = new Random();
        //    rndCount.Next(5, 10);
            
        //    for (int i = 0; i< rndCount.NextInt64(); i++)
        //    {
        //        if (i==3)
        //        {
        //            word += Password;
        //        }
        //        word += ((char)rnd.Next('A', 'Z')).ToString();
        //    }

        //    var isRegist = _userRepository.UserExistsAsync(username).Result;
        //    if (!isRegist)
        //    {
        //        await _wordRepository.CreateAsync(new Word { UserId = username, WordText = Password });

        //    }

        //    return word;

        //}

      




    }


}
