using Microsoft.EntityFrameworkCore;
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
        private readonly DbContext _dbContext;



        public UserRepository(IMongoDatabase database, DbContext dbContext)
        {
            _userCollection = database.GetCollection<User>("Users"); // Koleksiyon adı: Users
            _dbContext = dbContext;
        }

        #region Mongo 
        public async Task CreateAsyncMongo(User user) // Yeni kullanıcı eklemek için
        {
            await _userCollection.InsertOneAsync(user);
        }

        public async Task<User> GetByUsernameAsyncMongo(string username) // Kullanıcıyı kullanıcı adına göre getirmek için
        {
            return await _userCollection
                .Find(user => user.Username == username)
                .FirstOrDefaultAsync();
        }

        public async Task<User> LoginUserAsyncMongo(string username, string password)
        {
            return await _userCollection
            .Find(user => user.Username == username
                  && user.Password == password )
            .FirstOrDefaultAsync();            
        }

        public async Task<bool> UserExistsAsyncMongo(string username) // Kullanıcı var mı kontrolü
        {
            var count = await _userCollection
                .CountDocumentsAsync(user => user.Username == username);
            return count > 0;
        }
        #endregion

        public async Task CreateAsyncPostgre(User user)
        {
            await _dbContext.Set<User>().AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> GetByUsernameAsyncPostgre(string username)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> LoginUserAsyncPostgre(string username, string password)
        {
            return await _dbContext.Set<User>()
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<bool> UserExistsAsyncPostgre(string username)
        {
            return await _dbContext.Set<User>().AnyAsync(u => u.Username == username);
        }
    }
}
