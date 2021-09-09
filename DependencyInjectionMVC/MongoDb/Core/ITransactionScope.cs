using System;
using System.Threading.Tasks;

namespace MongoDb.Core
{
   public interface ITransactionScope : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        Task CommitTransactionAsync();
    }
}
