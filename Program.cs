using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using ORM.Context;

string connectionString = "Server=localhost;Database=master;Trusted_Connection=True;";
using (SqlConnection sqlConnection = new SqlConnection(connectionString))
{
    sqlConnection.Open();
    DbContext dbContext = new DbContext(sqlConnection);

}