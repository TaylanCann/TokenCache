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
       
        Task CreateAsyncMongo(User user);
        Task<User> GetByUsernameAsyncMongo(string username);
        Task<User> LoginUserAsyncMongo(string username, string password);
        Task<bool> UserExistsAsyncMongo(string username);

        Task CreateAsyncPostgre(User user);
        Task<User> GetByUsernameAsyncPostgre(string username);
        Task<User> LoginUserAsyncPostgre(string username, string password);
        Task<bool> UserExistsAsyncPostgre(string username);

    }
}
