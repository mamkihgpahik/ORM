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
    internal class UnitOfWork:IUnitOfWork
    {
        
        DbContext db;
        public UnitOfWork(DbContext dbContext)
        {
            db = dbContext;
        }
        private ShipRepository _shipRepository;
        private DirectionRepository _directionRepository;
       
        public ShipRepository ShipRepository
        {
            get
            {
                if (_shipRepository == null)
                    _shipRepository = new ShipRepository(db);
                return _shipRepository;
            }
        }
        public DirectionRepository DirectionRepository
        {
            get
            {
                if (_directionRepository == null)
                    _directionRepository = new DirectionRepository(db);
                return _directionRepository;
            }
        }
        public void Save()
        {
            db.SaveChanges();
        }
        //public void Dispose()
        //{
        //    db.Close();
        //}
    }
}
