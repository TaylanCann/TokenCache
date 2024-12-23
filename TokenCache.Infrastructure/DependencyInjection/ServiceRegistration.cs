using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.Interfaces;
using TokenCache.Domain.Interfaces;
using TokenCache.Infrastructure.Cache;
using TokenCache.Infrastructure.Configurations;
using TokenCache.Infrastructure.Repositories;

namespace TokenCache.Infrastructure.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // MongoDB
            var mongoDatabase = MongoDbConfiguration.ConfigureMongoDatabase(configuration);
            services.AddSingleton(mongoDatabase);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IWordRepository, WordRepository>();

            // Redis
            var redisConnection = RedisConfiguration.ConfigureRedis(configuration);
            services.AddSingleton(redisConnection);
            services.AddScoped<IRedisCacheService, RedisCacheService>();
        }
    }
}
