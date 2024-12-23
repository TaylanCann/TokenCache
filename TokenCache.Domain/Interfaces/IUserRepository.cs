﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Entities;

namespace TokenCache.Domain.Interfaces
{
    public interface IUserRepository
    {
       
        Task CreateAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        Task<User> LoginUserAsync(string username, string password);
        Task<bool> UserExistsAsync(string username);

    }
}
