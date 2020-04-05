
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDb
{
   public static class BsonClassMapExtensions
    {
        public static BsonMemberMap MapStringObjectId<TEntity>(this BsonClassMap<TEntity> classMap)
            where TEntity: IEntity<string>
        {
            return classMap.MapIdMember(c => c.Id)
                .SetSerializer(new StringSerializer(BsonType.ObjectId))
                .SetIdGenerator(StringObjectIdGenerator.Instance);
        }
    }
}
