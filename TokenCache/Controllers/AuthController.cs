using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync([FromBody] User request)
        {
            var redisCheck = await _redisCacheService.GetAsync(request.Username);

            if (!string.IsNullOrEmpty(redisCheck))
            {
                return Ok(new UserLoginResponse
                {
                    AuthToken = redisCheck
                });
            }

            var result = await _authService.LoginAsync(request.Username, request.Password);

            await _redisCacheService.SetAsync(request.Username, result.AuthToken, TimeSpan.FromHours(1));

            return Ok(result);
        }
    }
}
