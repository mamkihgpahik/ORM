using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    public interface IDbSet<TEntity>
    {
        void Add(TEntity entity);
        void Delete (Guid id);
        TEntity GetById(Guid id);
        IEnumerable<TEntity> GetAll();
        
    }
}
