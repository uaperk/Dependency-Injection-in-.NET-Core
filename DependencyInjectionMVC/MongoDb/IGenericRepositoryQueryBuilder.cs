using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb
{
   public interface IGenericRepositoryQueryBuilder<T>
    {

        Task<IReadOnlyList<T>> ToList();

        Task<T> FirstOrDefault();

        IGenericRepositoryQueryBuilder<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);

        IGenericRepositoryQueryBuilder<T> Where(Expression<Func<T, bool>> expression);

    }
}
