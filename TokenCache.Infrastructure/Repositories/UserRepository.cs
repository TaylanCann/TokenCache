using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Domain.Entities;
using TokenCache.Domain.Interfaces;

namespace TokenCache.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("Users"); // Koleksiyon adı: Users
        }

        public async Task CreateAsync(User user) // Yeni kullanıcı eklemek için
        {
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<User> GetByUsernameAsync(string username) // Kullanıcıyı kullanıcı adına göre getirmek için
        {
            return await _userCollection
                .Find(user => user.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            return await _userCollection
            .Find(user => user.Username == username
                  && user.Password == password )
            .FirstOrDefaultAsync();            
        }

        public async Task<bool> UserExistsAsync(string username) // Kullanıcı var mı kontrolü
        {
            var count = await _userCollection
                .CountDocumentsAsync(user => user.Username == username);
            return count > 0;
        }
    }
}
