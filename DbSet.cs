using ORM.Interfaces;
using ORM.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class DbSet<T>where T : class
    {
        
        QueryBuilder<T> qb { get; set; }
        public StringBuilder query { get; set; }
        public StringBuilder querySelect { get; set; }
        public DbSet(string database)
        {
            query = new StringBuilder();
            qb = new QueryBuilder<T>(database);
        }
        public void Add(T entity)
        {
            query.Append(qb.QueryToInsert(entity));
        }
        //public T Get(Guid id)
        //{
        //    return qb.QueryToGetById(id);
        //}
        public void Delete(Guid id)
        {
            query.Append(qb.QueryToDeleteById(id));
        }
        //public IEnumerable<T> Select()
        //{
        //    querySelect.Append(qb.QueryToGetAll());
        //}
    }
}
