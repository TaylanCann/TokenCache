using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(string username);
        ClaimsPrincipal ValidateTokenAsync(string token);
    }
}
