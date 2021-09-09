using System.Threading.Tasks;

namespace MongoDb.Core
{
   public interface IDatabaseConnection
    {
        Task<ITransactionScope> BeginTransactionScopeAsync();

        ITransactionScope BeginTransactionScope();
    }
}
