using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenCache.Application.Interfaces;
using TokenCache.Interfaces;
using TokenCache.Models;

namespace TokenCache.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        readonly IAuthService _authService;
        private readonly IRedisCacheService _redisCacheService;


        public AuthController(IAuthService authService,IRedisCacheService redisCacheService)
        {
            _authService = authService;
            _redisCacheService = redisCacheService;

        }

        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ActionResult<UserLoginResponse>> LoginUserAsync([FromBody] User request)
        {
            var redisCheck = await _redisCacheService.GetAsync(request.Username);

            if (!redisCheck.IsNullOrEmpty())
            {
                return BadRequest();
            }

            var result = await _authService.LoginUserAsync(request);
            await _redisCacheService.SetAsync(request.Username, result.AuthToken, TimeSpan.FromHours(1));

            return result;
        }
    }
}
