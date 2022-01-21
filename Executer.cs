using ORM.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class Executer<T>
    {
        SqlConnection conn;
        List<SqlTransaction> transactions;
        QueryBuilder<T> queryBuilder;
        public Executer(SqlConnection consql)
        { 
            this.conn = consql;
            transactions = new List<SqlTransaction>();
            queryBuilder = new QueryBuilder<T>(consql.Database);
        }

        public void GetData()
        {
            using (conn)
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SqlTransaction transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
          
                cmd.CommandText = "Select * from {TableName}";//ExecuteReader
                //cmd.CommandText = queryBuilder.Select();
                cmd.ExecuteReader();
                transactions.Add(cmd.Transaction);
                
            }
        }

    }
}
