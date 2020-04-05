using MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionMVC.Models
{
    public class Post : IEntity<string>
    {
        public string Id { get; set; }

        public DateTime PostDateTime { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
