using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb
{
   public interface IGenericRepositoryQueryBuilder<T>
    {

        Task<IReadOnlyList<T>> ToListAsync();

        IReadOnlyList<T> ToList();

        Task<T> FirstOrDefaultAsync();

        T FirstOrDefault();

        IGenericRepositoryQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);

        IGenericRepositoryQueryBuilder<TResult> SelectMany<TResult>(Expression<Func<T, IEnumerable<TResult>>> selector);
        IGenericRepositoryQueryBuilder<TCustomResult> SelectMany<TResult, TCustomResult>(Expression<Func<T, IEnumerable<TResult>>> collectionSelector, Expression<Func<T, TResult, TCustomResult>> resultSelector);

        IGenericRepositoryQueryBuilder<T> Where(Expression<Func<T, bool>> expression);

        bool Any(Expression<Func<T, bool>> predicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        IGenericRepositoryQueryBuilder<T> Distinct();

        IGenericRepositoryQueryBuilder<IGrouping<TKey, T>> GroupBy<TKey>(Expression<Func<T, TKey>> keySelector);

        Task<long> LongCountAsync();

        IGenericRepositoryQueryBuilder<TResult> OfType<TResult>();
        IGenericRepositoryQueryBuilder<T> OrderBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IGenericRepositoryQueryBuilder<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> keySelector);

        IGenericRepositoryQueryBuilder<T> Skip(int count);

        decimal Sum(Expression<Func<T, decimal>> selector);
        int Sum(Expression<Func<T, int>> selector);
        Task<int> SumAsync(Expression<Func<T, int>> selector);
        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector);

        IGenericRepositoryQueryBuilder<T> Take(int count);

        IGenericRepositoryQueryBuilder<T> ThenBy<TKey>(Expression<Func<T, TKey>> keySelector);
        IGenericRepositoryQueryBuilder<T> ThenByDescending<TKey>(Expression<Func<T, TKey>> keySelector);
    }
}
