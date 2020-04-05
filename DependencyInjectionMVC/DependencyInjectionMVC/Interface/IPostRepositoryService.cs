using DependencyInjectionMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionMVC.Interface
{
   public interface IPostRepositoryService
    {
        Task Create(Post model);

        IReadOnlyList<Post> GetAll();
    }
}
