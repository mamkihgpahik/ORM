using ORM.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    internal interface IRepository<T>
    {
        
        T Get(Guid id);
        IEnumerable<T> GetAll();
        void Add(T item);
        void Delete(Guid id);
        void Update(T item);
    }
}
