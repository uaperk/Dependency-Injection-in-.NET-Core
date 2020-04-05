using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDb
{
   public interface IEntity<TId>
        where TId : IEquatable<TId>
    {
        TId Id { get; }
    }
}
