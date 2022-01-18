using ORM.Context;
using ORM.Entities;
using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Repository
{
    internal class Repository : IRepository<Ship>
    {
        DbContext db;
        public Repository(DbContext dbContext)
        {
            db = dbContext;
        }
        public void Add(Ship item)
        {
            db.Ships.Add(item);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ship> GetAll()
        {
            throw new NotImplementedException();
        }

        public Ship Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Ship item)
        {
            throw new NotImplementedException();
        }
    }
}
