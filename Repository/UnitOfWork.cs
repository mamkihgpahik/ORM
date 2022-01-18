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
    internal class UnitOfWork : IUnitOfWork
    {
        public IRepository<Entity> Entity { get; set; }
        DbContext _dbContext;
        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
