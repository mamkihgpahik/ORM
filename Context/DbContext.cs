using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data;
using ORM.Entities;
using System.Data.SqlClient;
using ORM.Repository;
using ORM.Attributes;
using System.Reflection;

namespace ORM.Context
{
    public class DbContext
    {
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Ship> Ships { get; set; }
        private SqlConnection _connection;
        private string _database;
        private List<SqlTransaction> _transactions;
        //public Dictionary<Type, List<DbSet<Type>>> keyValuePairs { get; set; }

        public QueryBuilder<Direction> qb;
        //на контекст надо передавать лист транзакций из дб сета
        public DbContext(SqlConnection dbConnection)
        {
            _connection = dbConnection;
            _database = dbConnection.Database;
            _transactions = new List<SqlTransaction>();
        }
        //public void Add(T entity)
        //{
        //    try
        //    {
        //        StringBuilder SqlQuery = new StringBuilder();
        //        SqlQuery.Append(_queryBuilder.QueryToInsert(entity));
        //        /*var queryResult =*/ ExecuteSqlQuery(SqlQuery.ToString());
        //        //return new OperationCreateResult() { status = queryResult.status, exception = queryResult.exception, InsertedId = queryResult.InsertedId };
        //    }
        //    catch (Exception ex)
        //    {
        //        //return new OperationCreateResult() { status = Status.Failed, exception = ex };
        //    }
        //}
        //public void Delete(Guid id)
        //{
        //    try
        //    {
        //        StringBuilder SqlQuery = new StringBuilder();
        //        SqlQuery.Append(_queryBuilder.QueryToDeleteById(id));
        //        /*var queryResult = */
        //        ExecuteSqlQuery(SqlQuery.ToString());
        //        //return new OperationResult() { status = queryResult.status, exception = queryResult.exception };
        //    }
        //    catch (Exception ex)
        //    {
        //        //return new OperationResult() { status = Status.Failed, exception = ex };
        //    }
        //}
        //public void Update(T entity)
        //{
        //    try
        //    {

        //        StringBuilder SqlQuery = new StringBuilder();
        //        SqlQuery.Append(_queryBuilder.QueryToUpdate(entity));
        //        /*var queryResult =*/ ExecuteSqlQuery(SqlQuery.ToString());
        //        //return new OperationResult() { status = queryResult.status, exception = queryResult.exception };
        //    }
        //    catch (Exception ex)
        //    {
        //        //return new OperationResult() { status = Status.Failed, exception = ex };
        //    }
        //}





        private void ExecuteSqlQuery(string sqlQuery)//тут я заполняю лист транзакций
        {
            using (_connection)
            {
                try
                {
                    if (sqlQuery.Contains("INSERT"))
                    {
                        SqlCommand sqlCommand = new SqlCommand(sqlQuery, _connection);

                        _connection.Open();
                        var pID = new SqlParameter();
                        pID.ParameterName = "INSERTED_ID";
                        pID.Size = 128;
                        pID.Direction = ParameterDirection.Output;

                        sqlCommand.Parameters.Add(pID);

                        var insertedID = Convert.ToInt32(sqlCommand.ExecuteScalar());

                        //return new QueryResult() { status = Status.Succses, InsertedId = insertedID };
                    }
                    else
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, _connection);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        //return new QueryResult() { status = Status.Succses, Result = dataSet };
                    }
                }
                catch (SqlException ex)
                {
                    //return new QueryResult() { status = Status.Succses, exception = ex };
                }
            }
        }
        public void SaveChanges()//проходится по массиву всех дбсетов и выполнять там транзакции 
        {
            ExecuteTransaction();
        }
        private void ExecuteTransaction()//тут я выполняю все запросы которые были добавлены в транзакцию в методе ExecuteSqlQuery
        {
            using (SqlConnection connection = _connection)
            {
                SqlTransaction tran = connection.BeginTransaction();
                SqlCommand cmd = connection.CreateCommand();
                cmd.Transaction = tran;
                try
                {
                    //foreach (var item in keyValuePairs)
                    //{
                    //    foreach (var item1 in item.Value)
                    //    {
                    //        cmd.CommandText = $"{item1.query}";
                    //        cmd.ExecuteNonQuery();
                    //    }
                    //}
                    
                    tran.Commit();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    while (true)
                    {
                        tran.RollbackAsync();
                    }


                }
            }

            foreach (var item in _transactions)
            {
                try
                {
                    item.Commit();
                }
                catch (Exception)
                {
                    item.Rollback();
                    throw;
                }
                
            }
        }
       

    }
}

