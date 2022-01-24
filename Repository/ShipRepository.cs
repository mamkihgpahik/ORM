using ORM.Context;
using ORM.Entities;
using ORM.Interfaces;
using System.Linq.Expressions;

namespace ORM.Repository
{
    public class ShipRepository : IRepository<Ship>
    {
        private DbContext db;
        public ShipRepository(DbContext dbContext)
        {
                this.db = dbContext;
        }
        public void Add(Ship entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Ship Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ship> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Ship entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ship> Where(Expression<Func<Ship, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}