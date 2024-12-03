using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Domain.ValueObjects
{
    public class Password
    {
        public string Hash { get; private set; }

        public Password(string plainTextPassword, IPasswordHasher passwordHasher)
        {
            if (string.IsNullOrWhiteSpace(plainTextPassword))
                throw new ArgumentException("Password cannot be empty.");

            Hash = passwordHasher.HashPassword(plainTextPassword);
        }

        public bool Verify(string plainTextPassword, IPasswordHasher passwordHasher)
        {
            return passwordHasher.VerifyPassword(plainTextPassword, Hash);
        }
    }
}
