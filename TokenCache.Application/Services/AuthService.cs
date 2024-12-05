using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.DTOs;
using TokenCache.Application.Interfaces;
using TokenCache.Domain.Entities;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;  // Token işlemleri için
        private readonly IRedisCacheService _redisCacheService; // Redis işlemleri için

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IRedisCacheService redisCacheService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _redisCacheService = redisCacheService;
        }

        public async Task<UserDto> LoginAsync(string username, string password)
        {


            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null)
                return null;

            return new UserDto { Username = user.Username };
        }

       
        public async Task<UserDto> RegisterAsync(string username, string password)
        {
            if (await _userRepository.UserExistsAsync(username))
                throw new Exception("Username already exists."); // Username varsa hata fırlatıyoruz

            var user = new User(Guid.NewGuid().ToString(), username, password); // id oluşturuluyor
            await _userRepository.CreateAsync(user);

            return new UserDto { Username = user.Username }; // Başarılı kayıt
        }

        public async Task<string> SignInAsync(string username)
        {
            var token = await _tokenService.GenerateTokenAsync(username); // Token üret
            await _redisCacheService.SetAsync(username, token, TimeSpan.FromHours(1)); // Redis'e kaydet

            return token; // Token'ı geri döndür
        }       
    }
}
