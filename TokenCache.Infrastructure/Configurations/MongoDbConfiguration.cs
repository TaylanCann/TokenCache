using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Infrastructure.Configurations
{
    public static class MongoDbConfiguration
    {
        public static IMongoDatabase ConfigureMongoDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration["MongoDbSettings:DatabaseName"];
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        }
    }
}
