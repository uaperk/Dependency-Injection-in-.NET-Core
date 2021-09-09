using MongoDb.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb
{
    public class MongoDBTransactionScope : ITransactionScope
    {
        private bool isDisposed;
        private readonly MongoDBDatabaseConnection databaseConnection;

        public MongoDBTransactionScope(MongoDBDatabaseConnection databaseConnection, IClientSessionHandle session)
        {
            this.databaseConnection = databaseConnection;
            this.Session = session;
        }

        public IClientSessionHandle Session { get; }

        public void BeginTransaction()
        {
            this.Session.StartTransaction();
        }

        public void CommitTransaction()
        {
            this.Session.CommitTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            await this.Session.CommitTransactionAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.Session.IsInTransaction)
                {
                    this.Session.AbortTransaction();
                }

                this.Session.Dispose();
            }

            this.isDisposed = true;
        }
    }
}
