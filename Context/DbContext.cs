using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using ORM.Entities;

namespace ORM.Context
{
    internal class DbContext
    {
        IDbConnection _connection;
        public DbSet<Ship> Ships { get; set; }
        public DbContext(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public void SaveChanges() { }
    }
}
