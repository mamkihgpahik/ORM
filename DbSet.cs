using ORM.Attributes;
using ORM.Interfaces;
using ORM.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class DbSet<T> where T : class, new()
    {
        private SqlConnection _connection;
        private QueryBuilder<T> QueryBuilder;
        private List<SqlTransaction> _transactions;
        public DbSet(SqlConnection dbConnection)//для каждой сущности накапливаю транзакции и потом отправляю их на дб сетт, для гет методов возвращаю сразу значения через датасет
        {
            this._connection = dbConnection;
        }
        public void Add(T entity)
        {
            try
            {
                StringBuilder SqlQuery = new StringBuilder();
                SqlQuery.Append(QueryBuilder.QueryToInsert(entity));
                /*var queryResult =*/
                ExecuteSqlQuery(SqlQuery.ToString());
                //return new OperationCreateResult() { status = queryResult.status, exception = queryResult.exception, InsertedId = queryResult.InsertedId };
            }
            catch (Exception ex)
            {
                //return new OperationCreateResult() { status = Status.Failed, exception = ex };
            }
        }
        public void Delete(Guid id)
        {
            try
            {
                StringBuilder SqlQuery = new StringBuilder();
                SqlQuery.Append(QueryBuilder.QueryToDeleteById(id));
                /*var queryResult = */
                ExecuteSqlQuery(SqlQuery.ToString());
                //return new OperationResult() { status = queryResult.status, exception = queryResult.exception };
            }
            catch (Exception ex)
            {
                //return new OperationResult() { status = Status.Failed, exception = ex };
            }
        }
        public void Update(T entity)
        {
            try
            {

                StringBuilder SqlQuery = new StringBuilder();
                SqlQuery.Append(QueryBuilder.QueryToUpdate(entity));
                /*var queryResult =*/
                ExecuteSqlQuery(SqlQuery.ToString());
                //return new OperationResult() { status = queryResult.status, exception = queryResult.exception };
            }
            catch (Exception ex)
            {
                //return new OperationResult() { status = Status.Failed, exception = ex };
            }
        }
        //зарефакторить возвращаемый тип и доделать праивльную агрегацию 

        //public T Get(Guid id)
        //{
        //    try
        //    {
        //        StringBuilder sqlQuery = new StringBuilder("");
        //        sqlQuery.Append( _queryBuilder.QueryToGetById(id));

        //        var queryResult = ExecuteSqlQuery(sqlQuery.ToString());

        //        if (queryResult.status == Status.Failed)
        //            return null;

        //        var dataTable = queryResult.Result.Tables[0];

        //        T t = new T();
        //        T result = t;
        //        var props = typeof(T).GetProperties();

        //        if (dataTable.Rows.Count == 0)
        //            return null;

        //        for (int i = 0; i < dataTable.Columns.Count; i++)
        //        {
        //            var value = dataTable.Rows[0].ItemArray[i];
        //            props[i].SetValue(result, value);
        //        }

        //        IncludeForeignEntities(result);
        //        //return new OperationGetResult<T>() { status = queryResult.status, exception = queryResult.exception, result = result };
        //    }
        //    catch (Exception ex)
        //    {
        //        //return new OperationGetResult<T>() { status = Status.Failed, exception = ex };
        //    }

        //}

        //public IEnumerable<T> GetAll()
        //{
        //    try
        //    {
        //        string sqlQuery = _queryBuilder.QueryToGetAll();
        //        var prop = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //        var queryResult = ExecuteSqlQuery(sqlQuery);

        //        if (queryResult.status == Status.Failed)
        //            return null;

        //        var dataTable = queryResult.Result.Tables[0];

        //        List<T> items = new List<T>();

        //        for (int i = 0; i < dataTable.Rows.Count; i++)
        //        {
        //            T entity = new T();
        //            for (int j = 0; j < dataTable.Columns.Count; j++)
        //            {
        //                var value = dataTable.Rows[i].ItemArray[j];
        //                prop[j].SetValue(entity, value);
        //            }
        //            IncludeForeignEntities(entity);
        //            items.Add(entity);
        //        }

        //        return new OperationEnumerableResult<T>() { status = queryResult.status, exception = queryResult.exception, result = items };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new OperationEnumerableResult<T>() { status = Status.Failed, exception = ex };
        //    }
        //}

        //public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        //{
        //    try
        //    {
        //        var prop = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //        string sqlQuery = _queryBuilder.GenerateSqlQueryToWhere(predicate);

        //        var queryResult = ExecuteSqlQuery(sqlQuery);

        //        if (queryResult.status == Status.Failed)
        //            return null;

        //        var result = queryResult.Result;

        //        var dt = result.Tables[0];

        //        List<T> items = new List<T>();

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            T entity = new T();
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                var t1 = dt.Rows[i].ItemArray[j];

        //                prop[j].SetValue(entity, t1);
        //            }

        //            IncludeForeignEntities(entity);
        //            items.Add(entity);
        //        }

        //        return new OperationEnumerableResult<T>() { status = queryResult.status, exception = queryResult.exception, result = items };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new OperationEnumerableResult<T>() { status = Status.Failed, exception = ex };
        //    }
        //}
        //
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
        private QueryResult ExecuteSqlQueryResult(string sqlQuery)//тут я заполняю лист транзакций   
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

                        return new QueryResult() { status = Status.Succses, InsertedId = insertedID };
                    }
                    else
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, _connection);
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return new QueryResult() { status = Status.Succses, Result = dataSet };
                    }
                }
                catch (SqlException ex)
                {
                    return new QueryResult() { status = Status.Succses, exception = ex };
                }
            }

        }

        //private void IncludeForeignEntities(T mainEntity)
        //{
        //    var props = typeof(T).GetProperties();

        //    foreach (var p in props)
        //    {
        //        if (p.GetCustomAttribute<OneToManyAttribute>() != null)
        //        {
        //            var entity = p.PropertyType.GenericTypeArguments[0];

        //            Type[] typeArgs = { entity };

        //            string sqlQuery2 = QueryBuilder.GenerateSqlQueryOneToMany(Convert.ToInt32(props.Single(p => p.GetCustomAttribute<PrimaryKeyAttribute>() != null).GetValue(mainEntity)), p);
        //            var queryResult2 = ExecuteSqlQuery(sqlQuery2);

        //            if (queryResult2.status == Status.Failed)
        //                break;

        //            var dataTable2 = queryResult2.Result.Tables[0];

        //            object list = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeArgs));

        //            var entityProps = entity.GetProperties();

        //            var AddMethod = list.GetType().GetMethod("Add");

        //            for (int i = 0; i < dataTable2.Rows.Count; i++)
        //            {
        //                var item = Activator.CreateInstance(entity);

        //                for (int j = 0; j < dataTable2.Columns.Count; j++)
        //                {
        //                    var value = dataTable2.Rows[i].ItemArray[j];
        //                    entityProps[j].SetValue(item, value);
        //                }

        //                AddMethod.Invoke(list, new object[] { item });
        //            }

        //            p.SetValue(mainEntity, list);
        //        }
        //    }
        //}

    }
}
