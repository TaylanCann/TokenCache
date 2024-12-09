using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.DTOs.UserDTOs;

namespace TokenCache.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserDto> RegisterAsync(string username, string password);
        Task<UserDto> LoginAsync(string username, string password);
    }
}
