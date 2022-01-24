using ORM.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    public interface IRepository<T>
    {
        T Get(Guid id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Delete(Guid id);
        void Update(T entity);
        IEnumerable<T> Where(Expression<Func<T, bool>> predicate);
    }
}
