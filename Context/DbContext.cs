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

namespace ORM.Context
{
    public class DbContext
    {
        SqlConnection _connection;
        
        public DbSet<Ship> Ships { get; set; }

        public DbSet<Direction> Directions { get; set; }

        //public Dictionary<Type, List<DbSet<Type>>> keyValuePairs { get; set; }

        public QueryBuilder<Direction> qb;

        public DbContext(SqlConnection dbConnection, string databse)//как получить название бд имея строку подключения
        {
            _connection = dbConnection;
            Directions = new DbSet<Direction>(databse);
        }
        public void Add(Type type, object entity)
        {
            //if (entity.GetType() is type)
            //{
            //  keyValuePairs[type].Add();
            //}


        }
        public void SaveChanges()
        {
            //просто чтобы прога работала 
            List<SqlTransaction> transactions = new List<SqlTransaction>(); 
            //либо тут проходится по всем сгенерированным строкам из Executer/QueryBuilder
            ExecuteTransaction(transactions);
        }
        private void ExecuteTransaction(List<SqlTransaction> trans)
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
                    cmd.CommandText = $"{Directions.query}";
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

            foreach (var item in trans)
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
        public void Select()
        {
            using (SqlConnection connection = _connection)
            {

                // Создаем объект DataAdapter
                SqlDataAdapter adapter = new SqlDataAdapter(Directions.querySelect.ToString(), connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                adapter.Fill(ds);


                DataTable dt = ds.Tables[0];
                // добавим новую строку
                DataRow newRow = dt.NewRow();

                // Изменим значение в столбце Age для первой строки

                // Отображаем данные
                // перебор всех таблиц
                foreach (DataColumn column in dt.Columns)
                    Console.Write($"{column.ColumnName}\t");
                Console.WriteLine();
                // перебор всех строк таблицы
                foreach (DataRow row in dt.Rows)
                {
                    // получаем все ячейки строки
                    var cells = row.ItemArray;
                    foreach (object cell in cells)
                        Console.Write($"{cell}\t");
                    Console.WriteLine();
                }
            }
            Console.Read();
        }

    }
}

