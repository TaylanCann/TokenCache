using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Application.Interfaces
{
    public interface IRedisCacheService
    {
        Task SetAsync(string key, string value, TimeSpan expiration);
        Task<string> GetAsync(string key);
        Task<bool> ExistsAsync(string key);
        Task ClearAsync(string key);
        void ClearAll();


    }
}
