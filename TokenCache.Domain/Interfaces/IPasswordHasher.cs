using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Domain.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string plainTextPassword);
    }
}
