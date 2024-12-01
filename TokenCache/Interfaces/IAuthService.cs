using TokenCache.Models;

namespace TokenCache.Interfaces
{
    public interface IAuthService
    {
        public Task<UserLoginResponse> LoginUserAsync(User request);
    }
}
