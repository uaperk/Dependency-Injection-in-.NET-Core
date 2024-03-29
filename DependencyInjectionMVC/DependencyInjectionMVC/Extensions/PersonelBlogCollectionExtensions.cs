﻿using DependencyInjectionMVC.Interface;
using DependencyInjectionMVC.Models;
using DependencyInjectionMVC.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDb;
using MongoDb.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionMVC.Extensions
{
    public static class PersonelBlogCollectionExtensions
    {
        public static IServiceCollection AddMongoOptionDriver(this IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<Post>(pm =>
           {
               pm.AutoMap();
               pm.MapIdProperty(p => p.Id)
               .SetSerializer(new StringSerializer(BsonType.ObjectId))
               .SetIdGenerator(StringObjectIdGenerator.Instance);
               pm.SetIgnoreExtraElements(true);
           });


            services.AddSingleton<IPostRepositoryService, PostRepositoryService>();

            return services;
        }

    }
}
