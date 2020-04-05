using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Pluralize.NET;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Crypto;

namespace MongoDb
{
    public sealed class MongoDbRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
         where TEntity : class, IEntity<TId>
         where TId : IEquatable<TId>
    {

        private static readonly Pluralizer Pluralizer = new Pluralizer();
        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        private readonly Lazy<IMongoCollection<TEntity>> lazyCollection;
        private string customCollectionName;

        public MongoDbRepository(IOptions<MongoDbOptions> options)
        {
            this.client = new MongoClient(CryptorEngine.Decrypt256(options.Value.ConnectionString));
            this.database = this.client.GetDatabase(CryptorEngine.Decrypt256(options.Value.Database));
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

                return this.database.ListCollectionNames(options).Any();
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

        public int Add(TEntity entity)
        {
            try
            {
                this.Collection.InsertOne(entity);
                return 1;
            }
            catch (Exception)
            {

                return 0;
            }
           
          
        }

        public bool CreateCollection()
        {
            if (this.CollectionExists)
            {
                return false;
            }

            this.database.CreateCollection(this.Collection.CollectionNamespace.CollectionName);
            return true;
        }

        public bool Delete(TEntity entity)
        {
           return this.Collection.DeleteOne(e => e.Id.Equals(entity.Id)).DeletedCount > 0 ? true : false;
        }

        public bool DropCollection()
        {
            if (!this.CollectionExists)
            {
                return false;
            }

            this.database.DropCollection(this.Collection.CollectionNamespace.CollectionName);
            return true;
        }

        public TEntity GetById(TId id)
        {
            return this.Collection.AsQueryable<TEntity>().Where(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public bool Update(TEntity entity)
        {
            return this.Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity).ModifiedCount > 0 ? true : false;

        }

        private IMongoCollection<TEntity> CreateCollectionFactory()
        {
            return this.database.GetCollection<TEntity>(this.GetCollectionName());
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
    }
}
