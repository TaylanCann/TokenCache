using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Infrastructure.Configurations;

namespace TokenCache.Infrastructure.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            _database = MongoDbConfiguration.ConfigureMongoDatabase(configuration);
        }

        public void InsertRecord<T>(string collectionName, T record)
        {
            // Primary veritabanına kayıt
            var collection = _database.GetCollection<T>(collectionName);
            collection.InsertOne(record);         
        }
    }

}
