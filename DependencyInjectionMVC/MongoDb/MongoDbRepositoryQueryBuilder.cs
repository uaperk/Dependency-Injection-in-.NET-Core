using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDb
{
    public class MongoDbRepositoryQueryBuilder<T> : IGenericRepositoryQueryBuilder<T>
    {
        private IMongoQueryable<T> mongoQueryable;

        internal MongoDbRepositoryQueryBuilder(IMongoQueryable<T> mongoQueryabl)
        {
            this.mongoQueryable = mongoQueryabl;
        }

        public async Task<T> FirstOrDefault()
        {
            return await this.mongoQueryable.FirstOrDefaultAsync();
        }

        public IGenericRepositoryQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return new MongoDbRepositoryQueryBuilder<TResult>(this.mongoQueryable.Select(selector));
        }

        public async Task<IReadOnlyList<T>> ToList()
        {
            return await this.mongoQueryable.ToListAsync();
        }

        public IGenericRepositoryQueryBuilder<T> Where(Expression<Func<T, bool>> expression)
        {
            this.mongoQueryable = this.mongoQueryable.Where(expression);
            return this;
        }
    }
}
