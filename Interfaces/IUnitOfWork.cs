using ORM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    internal interface IUnitOfWork
    {
        IRepository<Entity> Entity { get; }
        void Save();
    }
}
