using ORM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Interfaces
{
    public interface IUnitOfWork
    {
        
        void Save();
    }
}
