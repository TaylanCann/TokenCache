using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.Interfaces;

namespace TokenCache.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;

        public TokenService(IRedisCacheService redisCacheService, IConfiguration configuration) 
        {
            _redisCacheService = redisCacheService;
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(string username)
        {
            var key = _configuration["Jwt:SecretKey"];
            var claims = new[] { new Claim(ClaimTypes.Name, username) };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            // Token'ın geçerliliğini kontrol et (kendi doğrulama mantığınızı ekleyin)
            return await _redisCacheService.ExistsAsync(token);
        }
    }
}
