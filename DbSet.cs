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
        IQueryBuilder queryBuilder;
        QueryBuilder<T> qb;
        public StringBuilder query { get; set; }
        public DbSet()
        {

        }
        public void Add(T entity)
        {
            query.Append(qb.QueryToInsert(entity));
        }
    }
}
