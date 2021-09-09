using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Pluralize.NET;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Core;
using System.Linq.Expressions;

namespace MongoDb
{
    public sealed class MongoDbRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
         where TEntity : class, IEntity<TId>
         where TId : IEquatable<TId>
    {

        private static readonly Pluralizer Pluralizer = new Pluralizer();
        MongoDBDatabaseConnection databaseConnection;
        private readonly Lazy<IMongoCollection<TEntity>> lazyCollection;
        private string customCollectionName;

        public MongoDbRepository(IDatabaseConnection databaseConnection)
        {
            if (!(databaseConnection is MongoDBDatabaseConnection mongoDBDatabaseConnection))
            {
                throw new InvalidOperationException("IDatabaseConnection is not MongoDBDatabaseConnection!");
            }

            this.databaseConnection = mongoDBDatabaseConnection;
            this.lazyCollection = new Lazy<IMongoCollection<TEntity>>(() => this.CreateCollectionFactory());
        }

        public string CollectionName
        {
            get
            {
                return this.customCollectionName;
            }
            set
            {
                if (this.lazyCollection.IsValueCreated)
                {
                    throw new InvalidOperationException("Cannot set the CustomCollectionName after the collections is created!");
                }
            }
        }

        public bool CollectionExists
        {
            get
            {

                var filter = new BsonDocument("name", this.GetCollectionName());
                var options = new ListCollectionNamesOptions { Filter = filter };

                return this.databaseConnection.Database.ListCollectionNames(options).Any();
            }
        }

        public static string DefaultCollectionName
        {
            get
            {
                string plural = Pluralizer.Pluralize(typeof(TEntity).Name);
                return plural.Substring(0, 1).ToLowerInvariant() + plural.Substring(1);
            }
        }



        public bool CreateCollection()
        {
            if (this.CollectionExists)
            {
                return false;
            }

            this.databaseConnection.Database.CreateCollection(this.Collection.CollectionNamespace.CollectionName);
            return true;
        }

        public bool DropCollection()
        {
            if (!this.CollectionExists)
            {
                return false;
            }

            this.databaseConnection.Database.DropCollection(this.Collection.CollectionNamespace.CollectionName);
            return true;
        }
        public async Task<ITransactionScope> BeginTransactionScopeAsync()
        {
            return await this.databaseConnection.BeginTransactionScopeAsync();
        }

        public ITransactionScope BeginTransactionScope()
        {
            return this.databaseConnection.BeginTransactionScope();
        }

        private IMongoCollection<TEntity> CreateCollectionFactory()
        {
            return this.databaseConnection.Database.GetCollection<TEntity>(this.GetCollectionName());
        }

        private string GetCollectionName()
        {
            return this.CollectionName ?? DefaultCollectionName;
        }

        private IMongoCollection<TEntity> Collection
        {
            get { return this.lazyCollection.Value; }
        }

        public void CreateIndex(IndexKeysDefinition<TEntity> indexKeysDefinition, CreateIndexOptions createIndexOptions)
        {
            var element = new CreateIndexModel<TEntity>(indexKeysDefinition, createIndexOptions);
            this.Collection.Indexes.CreateOne(element);
        }

        public IGenericRepositoryQueryBuilder<TEntity> Query()
        {
            return new MongoDbRepositoryQueryBuilder<TEntity>(this.Collection.AsQueryable());
        }

        public async Task<bool> ExistsByIdAsync(TId id)
        {
            // https://stackoverflow.com/questions/8046029/how-to-tell-if-a-record-exists-in-mongo-collection-c
            int count = await this.Collection.AsQueryable().Where(c => c.Id.Equals(id)).CountAsync();
            return count > 0;
        }

        public string GenerateStringId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public void AddOne(TEntity entity, ITransactionScope transactionScope = null)
        {

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.InsertOne(session, entity);
            }
            else
            {
                this.Collection.InsertOne(entity);
            }

        }

        public async Task AddOneAsync(TEntity entity, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.InsertOneAsync(session, entity);
            }
            else
            {
                await this.Collection.InsertOneAsync(entity);
            }
        }

        public async Task AddManyAsync(ICollection<TEntity> entities, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.InsertManyAsync(session, entities);
            }
            else
            {
                await this.Collection.InsertManyAsync(entities);
            }
        }

        public void AddMany(ICollection<TEntity> entities, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.InsertMany(session, entities);
            }
            else
            {
                this.Collection.InsertMany(entities);
            }
        }

        public TEntity GetById(TId id)
        {
            return this.Collection.AsQueryable<TEntity>().Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await this.Collection.AsQueryable().Where(c => c.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public void Update(TEntity entity, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.ReplaceOne(session, e => e.Id.Equals(entity.Id), entity);
            }
            else
            {
                this.Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity);
            }
        }

        public async Task UpdateAsync(TEntity entity, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.ReplaceOneAsync(session, e => e.Id.Equals(entity.Id), entity);
            }
            else
            {
                await this.Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
            }
        }

        public void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value, ITransactionScope transactionScope = null)
        {
            UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>().Set(field, value);

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.UpdateMany(session, filter, updateDefinition);
            }
            else
            {
                this.Collection.UpdateMany(filter, updateDefinition);
            }
        }

        public void UpdateMany<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null)
        {
            // TODO: su anda coklu update icin butun fieldlerin ayni tipte olmasi gerekiyor, ya da TField olarak object kullanmak
            // bunun yerine farklı bir update yapisi koymak lazim, boylece projector generic repository kullanabilir
            var builder = new UpdateDefinitionBuilder<TEntity>();

            UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.UpdateMany(session, filter, updateDefinition);
            }
            else
            {
                this.Collection.UpdateMany(filter, updateDefinition);
            }
        }

        public async Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value, ITransactionScope transactionScope = null)
        {
            UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>().Set(field, value);

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.UpdateManyAsync(session, filter, updateDefinition);
            }
            else
            {
                await this.Collection.UpdateManyAsync(filter, updateDefinition);
            }
        }


        public async Task UpdateManyAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null)
        {
            // TODO: su anda coklu update icin butun fieldlerin ayni tipte olmasi gerekiyor, ya da TField olarak object kullanmak
            var builder = new UpdateDefinitionBuilder<TEntity>();

            UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.UpdateManyAsync(session, filter, updateDefinition);
            }
            else
            {
                await this.Collection.UpdateManyAsync(filter, updateDefinition);
            }
        }

        public void Upsert(TEntity entity, ITransactionScope transactionScope = null)
        {
            var replaceOptions = new ReplaceOptions() { IsUpsert = true };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.ReplaceOne(session, e => e.Id.Equals(entity.Id), entity, replaceOptions);
            }
            else
            {
                this.Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity, replaceOptions);
            }
        }

        public async Task UpsertAsync(TEntity entity, ITransactionScope transactionScope = null)
        {
            var replaceOptions = new ReplaceOptions() { IsUpsert = true };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.ReplaceOneAsync(session, e => e.Id.Equals(entity.Id), entity, replaceOptions);
            }
            else
            {
                await this.Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity, replaceOptions);
            }
        }


        public async Task UpsertOneByFilterAsync<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null)
        {
            var builder = new UpdateDefinitionBuilder<TEntity>();

            UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));
            var updateOptions = new UpdateOptions() { IsUpsert = true };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.UpdateOneAsync(session, filter, updateDefinition, options: updateOptions);
            }
            else
            {
                await this.Collection.UpdateOneAsync(filter, updateDefinition, options: updateOptions);
            }
        }

        public void UpsertOneByFilter<TField>(Expression<Func<TEntity, bool>> filter, IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates, ITransactionScope transactionScope = null)
        {
            var builder = new UpdateDefinitionBuilder<TEntity>();

            UpdateDefinition<TEntity> updateDefinition = builder.Combine(updates.Select(update => builder.Set(update.Key, update.Value)));
            var updateOptions = new UpdateOptions() { IsUpsert = true };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.UpdateOne(session, filter, updateDefinition, options: updateOptions);
            }
            else
            {
                this.Collection.UpdateOne(filter, updateDefinition, options: updateOptions);
            }
        }

        public TEntity FindOneAndUpdate<TField>(
        Expression<Func<TEntity, bool>> filter,
        IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
        Expression<Func<TEntity, int>> increment,
        ITransactionScope transactionScope = null)
        {
            FilterDefinition<TEntity> filterDefinition = Builders<TEntity>.Filter.Where(filter);

            UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>()
                .Inc(increment, 1);

            foreach (KeyValuePair<Expression<Func<TEntity, TField>>, TField> field in updates)
            {
                updateDefinition.Set(field.Key, field.Value);
            }

            var options = new FindOneAndUpdateOptions<TEntity>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                return this.Collection.FindOneAndUpdate(
                    session,
                    filterDefinition,
                    updateDefinition,
                    options);
            }

            return this.Collection.FindOneAndUpdate(filterDefinition, updateDefinition, options);
        }

        public async Task<TEntity> FindOneAndUpdateAsync<TField>(
           Expression<Func<TEntity, bool>> filter,
           IEnumerable<KeyValuePair<Expression<Func<TEntity, TField>>, TField>> updates,
           Expression<Func<TEntity, int>> increment,
           ITransactionScope transactionScope = null)
        {
            FilterDefinition<TEntity> filterDefinition = Builders<TEntity>.Filter.Where(filter);

            UpdateDefinition<TEntity> updateDefinition = new UpdateDefinitionBuilder<TEntity>()
                .Inc(increment, 1);

            foreach (KeyValuePair<Expression<Func<TEntity, TField>>, TField> field in updates)
            {
                updateDefinition.Set(field.Key, field.Value);
            }

            var options = new FindOneAndUpdateOptions<TEntity>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };

            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                return await this.Collection.FindOneAndUpdateAsync(
                    session,
                    filterDefinition,
                    updateDefinition,
                    options);
            }

            return await this.Collection.FindOneAndUpdateAsync(filterDefinition, updateDefinition, options);
        }

        public void Delete(TId id, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.DeleteOne(session, e => e.Id.Equals(id));
            }
            else
            {
                this.Collection.DeleteOne(e => e.Id.Equals(id));
            }
        }

        public async Task DeleteAsync(TId id, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.DeleteOneAsync(session, e => e.Id.Equals(id));
            }
            else
            {
                await this.Collection.DeleteOneAsync(e => e.Id.Equals(id));
            }
        }

        public void DeleteMany(ICollection<TId> ids, ITransactionScope transactionScope = null)
        {
            this.DeleteMany(e => ids.Contains(e.Id), transactionScope);
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filter, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                this.Collection.DeleteMany(session, filter);
            }
            else
            {
                this.Collection.DeleteMany(filter);
            }
        }

        public async Task DeleteManyAsync(ICollection<TId> ids, ITransactionScope transactionScope = null)
        {
            await this.DeleteManyAsync(e => ids.Contains(e.Id), transactionScope);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> filter, ITransactionScope transactionScope = null)
        {
            if (TryGetCurrentSession(transactionScope, out IClientSessionHandle session))
            {
                await this.Collection.DeleteManyAsync(session, filter);
            }
            else
            {
                await this.Collection.DeleteManyAsync(filter);
            }
        }

        private static bool TryGetCurrentSession(ITransactionScope transactionScope, out IClientSessionHandle session)
        {
            if (transactionScope is MongoDBTransactionScope mongoDBTransactionScope)
            {
                session = mongoDBTransactionScope.Session;
                return true;
            }

            session = null;
            return false;
        }
    }
}
