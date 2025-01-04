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
        private readonly IMongoDatabase _primaryDatabase;
        private readonly IMongoDatabase _secondaryDatabase;

        public MongoDbService(IConfiguration configuration)
        {
            _primaryDatabase = MongoDbConfiguration.ConfigureMongoDatabase(configuration);
            _secondaryDatabase = MongoDbConfiguration.ConfigureTestMongoDatabase(configuration);
        }

        public void InsertRecord<T>(string collectionName, T record)
        {
            // Primary veritabanına kayıt
            var primaryCollection = _primaryDatabase.GetCollection<T>(collectionName);
            primaryCollection.InsertOne(record);

            // Secondary veritabanına kayıt
            var secondaryCollection = _secondaryDatabase.GetCollection<T>(collectionName);
            secondaryCollection.InsertOne(record);
        }
    }

}
