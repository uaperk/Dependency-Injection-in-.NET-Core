using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDb.Core;

namespace MongoDb
{
   public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoRepository(this IServiceCollection services)
        {
            
            services.AddSingleton(typeof(IGenericRepository<,>), typeof(MongoDbRepository<,>));
            services.AddSingleton(typeof(IDatabaseConnection), typeof(MongoDBDatabaseConnection));
            return services;
        }
    }
}
