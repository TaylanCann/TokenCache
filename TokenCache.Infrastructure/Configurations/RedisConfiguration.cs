using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TokenCache.Infrastructure.Configurations
{
    public static class RedisConfiguration
    {
        public static IConnectionMultiplexer ConfigureRedis(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Redis");
            return ConnectionMultiplexer.Connect(connectionString);
        }
    }
}
