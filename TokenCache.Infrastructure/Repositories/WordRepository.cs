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
    public class WordRepository : IWordRepository
    {
        private readonly IMongoCollection<Word> _wordCollection;

        public WordRepository(IMongoCollection<Word> wordCollection)
        {
            _wordCollection = wordCollection;
        }

        public async Task CreateAsync(Word word)
        {
            await _wordCollection.InsertOneAsync(word);
        }

        public async Task<Word> GetByIdAsync(int Id)
        {
            return await _wordCollection
              .Find(word => word.UserId == Id)
              .FirstOrDefaultAsync();

        }
    }
}
