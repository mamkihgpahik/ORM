using ORM.Context;
using ORM.Entities;
using ORM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Repository
{
    public class DirectionRepository:IRepository<Direction>
    {
        private DbContext db;
        public DirectionRepository(DbContext dbContext)
        {
            this.db = dbContext;    
        }

        public void Add(Direction entity)
        {
            db.Directions.Add(entity);
        }

        public void Delete(Guid id)
        {
            db.Directions.Delete(id);
        }

        public void Update(Direction entity)
        {
            db.Directions.Update(entity);
        }

        public Direction Get(Guid id)
        {
            return db.Directions.Get(id);
        }

        public IEnumerable<Direction> GetAll()
        {
            return db.Directions.GetAll();
        }

        public IEnumerable<Direction> Where(Expression<Func<Direction, bool>> predicate)
        {
            return db.Directions.Where(predicate);
        }
    }
}
