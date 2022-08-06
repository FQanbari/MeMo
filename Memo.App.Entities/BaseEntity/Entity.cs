using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memo.App.Entities.BaseEntity
{
    public interface IEntity { }
    public abstract class Entity<TKey> : IEntity
    {
        public TKey Id { get; set; }
    }
    public abstract class Entity : Entity<int>
    {
    }
}
