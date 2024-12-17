using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TokenCache.Application.DTOs.UserDTOs;
using TokenCache.Application.Interfaces;
using TokenCache.Domain.Entities;

namespace TokenCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRedisCacheService _redisCacheService;

        public AuthController(IAuthService authService, IRedisCacheService redisCacheService)
        {
            _authService = authService;
            _redisCacheService = redisCacheService;
        }

        [HttpPost("LoginUserAsync")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginAsync([FromBody] UserAuthDto request)
        {
           
            var result = await _authService.LoginAsync(request.Username, request.Password);

            return Ok(result);
        }

        [HttpPost("RegisterUserAsync")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> RegisterUserAsync([FromBody] UserAuthDto request)
        {
           
            var result = await _authService.RegisterAsync(request.Username, request.Password);          

            return Ok(result);
        }
    }
}
