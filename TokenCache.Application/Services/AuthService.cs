using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.DTOs.UserDTOs;
using TokenCache.Application.Exceptions;
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
                throw UserNotFoundException.UserNotFoundForLogin(username);
            
            var token = await _redisCacheService.GetAsync(username);
            string hassedPassword;
            if (token == null)
            {
                hassedPassword = _passwordHasher.HashPassword(user,password);
                user = await _userRepository.LoginUserAsync(username, hassedPassword);

                token = await _tokenService.GenerateTokenAsync(username); // Token üret
                var a = _tokenService.ValidateTokenAsync(token);
                await _redisCacheService.SetAsync(username, token, TimeSpan.FromHours(1)); // Redis'e kaydet
            }

            hassedPassword = _passwordHasher.HashPassword(user,password);
            user = await _userRepository.LoginUserAsync(username, hassedPassword);
            if (user!=null)
            {
                return new UserDto
                {
                    Username = user.Username,
                    AuthToken = token,
                    AccessTokenExpireDate = DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                    AuthenticateResult = true
                };
            }


            return new UserDto
            {
                Username = user.Username,
                AuthToken = token,
                AccessTokenExpireDate = DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                AuthenticateResult = true
            };


        }

       
        public async Task<UserDto> RegisterAsync(string username, string password)
        {
            if (await _userRepository.UserExistsAsync(username))
                throw new Exception("Username already exists."); 

            var hassedPassword = _passwordHasher.HashPassword(password);
            var user = new User(Guid.NewGuid().ToString(), username, hassedPassword); 
            await _userRepository.CreateAsync(user);

            var token = await _tokenService.GenerateTokenAsync(username); 
            await _redisCacheService.SetAsync(username, token, TimeSpan.FromHours(1)); 

            return new UserDto 
            { 
              Username = user.Username,
              AuthToken = token,
              AccessTokenExpireDate = DateTime.UtcNow.Add(TimeSpan.FromHours(1)), 
              AuthenticateResult = true 
            }; // Başarılı kayıt
        }

       
    }
}
