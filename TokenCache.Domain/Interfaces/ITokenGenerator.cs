using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Domain.Interfaces
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username);
    }
}
