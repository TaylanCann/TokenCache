using TokenCache.Models;

namespace TokenCache.Interfaces
{
    public interface ITokenService
    {
        public Task<GenerateTokenResponse> GenerateTokenAsync(GenerateTokenRequest request);
    }
}
