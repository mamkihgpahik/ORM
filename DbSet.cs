using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    internal class DbSet<T>where T : class
    {
        IQueryBuilder queryBuilder;
        public DbSet()
        {

        }
        public void Add(T entity)
        {
            
        }
    }
}
