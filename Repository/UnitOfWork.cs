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
    internal class UnitOfWork
    {
        
        DbContext db;
        public UnitOfWork(DbContext dbContext)
        {
            db = dbContext;
        }
        private ShipRepository _shipRepository;

        public ShipRepository ShipRepository
        {
            get
            {
                if (_shipRepository == null)
                    _shipRepository = new ShipRepository(db);
                return _shipRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}
