using MongoDb.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MongoDb
{
    public interface IGenericRepository<TEntity, TId>
         where TEntity : class
         where TId : IEquatable<TId>
    {

        string CollectionName { get; set; }

        bool CollectionExists { get; }

        bool CreateCollection();

        bool DropCollection();

        ITransactionScope BeginTransactionScope();
        Task<ITransactionScope> BeginTransactionScopeAsync();

        IGenericRepositoryQueryBuilder<TEntity> Query();

        Task<bool> ExistsByIdAsync(TId id);

        TEntity GetById(TId id);

        Task<TEntity> GetByIdAsync(TId id);


        string GenerateStringId();


        void AddOne(TEntity entity, ITransactionScope transactionScope = null);

        Task AddOneAsync(TEntity entity, ITransactionScope transactionScope = null);

        void AddMany(ICollection<TEntity> entities, ITransactionScope transactionScope = null);
        Task AddManyAsync(ICollection<TEntity> entities, ITransactionScope transactionScope = null);

        void Update(TEntity entity, ITransactionScope transactionScope = null);

        Task UpdateAsync(TEntity entity, ITransactionScope transactionScope = null);

        void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value, ITransactionScope transactionScope = null);

        void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null);

        Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value, ITransactionScope transactionScope = null);
        Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null);

        void Upsert(TEntity entity, ITransactionScope transactionScope = null);
        Task UpsertAsync(TEntity entity, ITransactionScope transactionScope = null);

        void UpsertOneByFilter<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null);
        Task UpsertOneByFilterAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null);

        TEntity FindOneAndUpdate<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>> increment, ITransactionScope transactionScope = null);
        Task<TEntity> FindOneAndUpdateAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, Expression<Func<TEntity, int>> increment, ITransactionScope transactionScope = null);

        void Delete(TId id, ITransactionScope transactionScope = null);

        Task DeleteAsync(TId id, ITransactionScope transactionScope = null);

        void DeleteMany(ICollection<TId> ids, ITransactionScope transactionScope = null);

        void DeleteMany(Expression<Func<TEntity, bool>> filter, ITransactionScope transactionScope = null);

        Task DeleteManyAsync(ICollection<TId> ids, ITransactionScope transactionScope = null);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, ITransactionScope transactionScope = null);

    }
}
