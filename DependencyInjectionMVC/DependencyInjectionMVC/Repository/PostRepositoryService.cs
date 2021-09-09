using DependencyInjectionMVC.Interface;
using DependencyInjectionMVC.Models;
using MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionMVC.Repository
{
    public class PostRepositoryService : IPostRepositoryService
    {
        private readonly IGenericRepository<Post, string> genericRepository;
        public PostRepositoryService(
            IGenericRepository<Post, string> genericRepository)
        {
            this.genericRepository = genericRepository;
        }

        public async Task Create(Post model)
        {
           this.genericRepository.AddOne(model);
        }

        public IReadOnlyList<Post> GetAll()
        {
            return  this.genericRepository.Query().ToListAsync().GetAwaiter().GetResult();
        }
    }
}
