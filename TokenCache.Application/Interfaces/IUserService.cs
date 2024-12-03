using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.DTOs;
using TokenCache.Domain.ValueObjects;

namespace TokenCache.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(string username, Password password);
        Task<UserDto> LoginAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);
    }
}
