using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Entities;

namespace TokenCache.Domain.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        Task CreateAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);

    }
}
