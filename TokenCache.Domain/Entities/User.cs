using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Domain.Entities
{
    public class User
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public User(string id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

       
    }
}
