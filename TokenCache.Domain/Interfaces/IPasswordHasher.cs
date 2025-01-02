using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Entities;

namespace TokenCache.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        Task<string> HashPassword(string password);
        //Task<string> SaltPassword(string username, string Password);
    }
}
