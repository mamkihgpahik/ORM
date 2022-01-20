using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using ORM.Entities;
using System.Data.SqlClient;

namespace ORM.Context
{
    public class DbContext
    {
        SqlConnection _connection;

        public DbSet<Ship> Ships { get; set; }
        public Dictionary<Type, List<DbSet<Type>>> keyValuePairs { get; set; }

        public DbContext(SqlConnection dbConnection)
        {
            _connection = dbConnection;
        }

        public void SaveChanges()
        {
            ExecuteTransaction();
        }
        private void ExecuteTransaction()
        {
            using (SqlConnection connection = _connection)
            {
                connection.Open();
                SqlTransaction tran = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = tran;
                try
                {
                    foreach (var item in keyValuePairs)
                    {
                        foreach (var item1 in item.Value)
                        {
                            cmd.CommandText = $"{item1.query}";
                        }
                    }
                    cmd.ExecuteNonQuery();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                   Console.WriteLine(ex.Message);
                   
                   tran.RollbackAsync();

                }
            }
        }
    }
}
