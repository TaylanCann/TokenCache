using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.ValueObjects;

namespace TokenCache.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string plainTextPassword);
        bool VerifyPassword(string plainTextPassword, string hashedPassword);
    }
}
