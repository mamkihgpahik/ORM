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
    public class DirectionRepository : IRepository<Direction>
    {
        private DbContext db;
        public DirectionRepository(DbContext dbContext)
        {
            this.db = dbContext;    
        }
        public void Add(Direction item)
        {
            db.Directions.Add(item);
        }

        public void Delete(Guid id)
        {
            db.Directions.Delete(id);
        }

        public Direction Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Direction> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Direction item)
        {
            throw new NotImplementedException();
        }
    }
}
