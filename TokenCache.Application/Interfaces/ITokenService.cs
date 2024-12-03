using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Application.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(string username);
        Task<bool> ValidateTokenAsync(string token);
    }
}
