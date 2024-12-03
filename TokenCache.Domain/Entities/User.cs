using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Interfaces;
using TokenCache.Domain.ValueObjects;

namespace TokenCache.Domain.Entities
{
    public class User
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public Password Password { get; private set; }

        public User(string id, string username, Password password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public bool VerifyPassword(string plainTextPassword, IPasswordHasher passwordHasher)
        {
            return Password.Verify(plainTextPassword, passwordHasher);
        }
    }
}
