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

        public async Task<T> FirstOrDefaultAsync()
        {
            return await this.mongoQueryable.FirstOrDefaultAsync();
        }

        public T FirstOrDefault()
        {
            // TODO: class ise T? dönebilmeli?
            return this.mongoQueryable.FirstOrDefault();
        }

        public IGenericRepositoryQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return new MongoDbRepositoryQueryBuilder<TResult>(this.mongoQueryable.Select(selector));
        }

        public IGenericRepositoryQueryBuilder<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector)
        {
            return new MongoDbRepositoryQueryBuilder<TResult>(this.mongoQueryable.SelectMany(selector));
        }

        public IGenericRepositoryQueryBuilder<TCustomResult> SelectMany<TResult, TCustomResult>(
            Expression<Func<T, IEnumerable<TResult>>> collectionSelector,
            Expression<Func<T, TResult, TCustomResult>> resultSelector)
        {
            return new MongoDbRepositoryQueryBuilder<TCustomResult>(
                this.mongoQueryable.SelectMany(collectionSelector, resultSelector));
        }

        public async Task<IReadOnlyList<T>> ToListAsync()
        {
            return await this.mongoQueryable.ToListAsync();
        }

        public IReadOnlyList<T> ToList()
        {
            return this.mongoQueryable.ToList();
        }

        public IGenericRepositoryQueryBuilder<T> Where(Expression<Func<T, bool>> expression)
        {
            this.mongoQueryable = this.mongoQueryable.Where(expression);
            return this;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return this.mongoQueryable.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await this.mongoQueryable.AnyAsync(predicate);
        }

        public IGenericRepositoryQueryBuilder<T> Distinct()
        {
            this.mongoQueryable = this.mongoQueryable.Distinct();
            return this;
        }

        public IGenericRepositoryQueryBuilder<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            return new MongoDbRepositoryQueryBuilder<IGrouping<TKey, T>>(mongoQueryable.GroupBy(keySelector));
        }

        public async Task<long> LongCountAsync()
        {
            return await this.mongoQueryable.LongCountAsync();
        }

        public IGenericRepositoryQueryBuilder<TResult> OfType<TResult>()
        {
            return new MongoDbRepositoryQueryBuilder<TResult>(this.mongoQueryable.OfType<TResult>());
        }

        public IGenericRepositoryQueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.mongoQueryable = this.mongoQueryable.OrderBy(keySelector);
            return this;
        }

        public IGenericRepositoryQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            this.mongoQueryable = this.mongoQueryable.OrderByDescending(keySelector);
            return this;
        }


        public IGenericRepositoryQueryBuilder<T> Skip(int count)
        {
            this.mongoQueryable = this.mongoQueryable.Skip(count);
            return this;
        }
        public decimal Sum(Expression<Func<T, decimal>> selector)
        {
            return this.mongoQueryable.Sum(selector);
        }

        public int Sum(Expression<Func<T, int>> selector)
        {
            return this.mongoQueryable.Sum(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector)
        {
            return await this.mongoQueryable.SumAsync(selector);


        }

        public async Task<int> SumAsync(Expression<Func<T, int>> selector)
        {
            return await this.mongoQueryable.SumAsync(selector);
        }

        public IGenericRepositoryQueryBuilder<T> Take(int count)
        {
            this.mongoQueryable = this.mongoQueryable.Take(count);
            return this;
        }

        public IGenericRepositoryQueryBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            if (!(this.mongoQueryable is IOrderedMongoQueryable<T> orderedMongoQueryable))
            {
                throw new InvalidOperationException(
                    $"{nameof(this.OrderBy)} or {nameof(this.OrderByDescending)} should be called before {nameof(this.ThenBy)}!");
            }

            this.mongoQueryable = orderedMongoQueryable.ThenBy(keySelector);
            return this;
        }

        public IGenericRepositoryQueryBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            if (!(this.mongoQueryable is IOrderedMongoQueryable<T> orderedMongoQueryable))
            {
                throw new InvalidOperationException(
                    $"{nameof(this.OrderBy)} or {nameof(this.OrderByDescending)} should be called before {nameof(this.ThenByDescending)}!");
            }

            this.mongoQueryable = orderedMongoQueryable.ThenByDescending(keySelector);
            return this;
        }


    }
}
