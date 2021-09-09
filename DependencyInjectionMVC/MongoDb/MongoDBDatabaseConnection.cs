using Crypto;
using Microsoft.Extensions.Options;
using MongoDb.Core;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MongoDb
{
    public class MongoDBDatabaseConnection : IDatabaseConnection
    {
        public MongoDBDatabaseConnection(IOptions<MongoDbOptions> options)
        {
            this.Client = new MongoClient(CryptorEngine.Decrypt256(options.Value.ConnectionString));
            this.Database = Client.GetDatabase(CryptorEngine.Decrypt256(options.Value.Database));
        }

        internal MongoClient Client;

        internal IMongoDatabase Database;

        public ITransactionScope BeginTransactionScope()
        {
            throw new System.NotImplementedException();
        }

        public Task<ITransactionScope> BeginTransactionScopeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
