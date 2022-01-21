using ORM.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Repository
{
    public class QueryBuilder<T>
    {
        public string DataBase { get; set; }

        private Dictionary<ExpressionType, string> nodeTypesToSql = new Dictionary<ExpressionType, string>();
        StringBuilder query = new StringBuilder();
        public QueryBuilder(string Database)
        {
            this.DataBase = Database;
            nodeTypesToSql.Add(ExpressionType.GreaterThan, ">");
            nodeTypesToSql.Add(ExpressionType.LessThan, "<");
            nodeTypesToSql.Add(ExpressionType.OrElse, "OR");
            nodeTypesToSql.Add(ExpressionType.Equal, "=");
            nodeTypesToSql.Add(ExpressionType.AndAlso, "AND");
            nodeTypesToSql.Add(ExpressionType.GreaterThanOrEqual, ">=");
            nodeTypesToSql.Add(ExpressionType.LessThanOrEqual, "<=");
        }

        public StringBuilder QueryToInsert(T entity)
        {
            var properties = entity.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            StringBuilder sqlQuery = new StringBuilder();

            //if (IsPrimaryKeyIsForeignKey())
            //{
            //    sqlQuery.Append($"INSERT INTO {DataBase}.dbo.{GetTableName()} (");

            //    for (int i = 0; i < properties.Length; i++)
            //    {
            //        sqlQuery.Append(properties[i].Name);

            //        if (i != properties.Length - 1)
            //            sqlQuery.Append(",");
            //    }

            //    sqlQuery.Append(")\n");
            //    sqlQuery.Append("VALUES(");

            //    for (int i = 0; i < properties.Length; i++)
            //    {
            //        if (IsNumber(properties[i].GetValue(entity)))
            //            sqlQuery.Append(properties[i].GetValue(entity));
            //        else
            //            sqlQuery.Append($"\'{properties[i].GetValue(entity)}\'");

            //        if (i != properties.Length - 1)
            //            sqlQuery.Append(",");
            //    }

            //    sqlQuery.Append(")");

            //    return sqlQuery;
            //}

            sqlQuery.Append($"INSERT INTO {DataBase}.dbo.{GetTableName()} (");

            for (int i = 1; i < properties.Length; i++)
            {
                sqlQuery.Append(properties[i].Name);

                if (i != properties.Length - 1)
                    sqlQuery.Append(",");
            }

            sqlQuery.Append(")\n");
            sqlQuery.Append("VALUES(");

            for (int i = 1; i < properties.Length; i++)
            {
                if (IsNumber(properties[i].GetValue(entity)))
                    sqlQuery.Append(properties[i].GetValue(entity));
                else
                    sqlQuery.Append($"\'{properties[i].GetValue(entity)}\'");

                if (i != properties.Length - 1)
                    sqlQuery.Append(",");
            }

            sqlQuery.Append(")\n");
            sqlQuery.Append("SELECT SCOPE_IDENTITY()");
            query.Append(sqlQuery.ToString());
            return sqlQuery;
        }

        public StringBuilder QueryToUpdate(T entity) { return new StringBuilder(); }

        public StringBuilder QueryToDeleteById(Guid id) { return new StringBuilder(); }

        public void QueryToGetById(Guid id)
        {
            StringBuilder sql = new StringBuilder();
            //if (IsPrimaryKeyIsForeignKey())
            //    return GenerateJoinPartQueryForRelation_1_to_1() + $"WHERE {GetTableName()}.{GetPrimaryKeyName()} = {id}";

            sql.Append($"SELECT TOP(1) * FROM {DataBase}.dbo.{GetTableName()} WHERE Id = {id}");
            Execute(sql);
        }

        public StringBuilder QueryToGetAll() 
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append($"SELECT * FROM {DataBase}.dbo.{GetTableName()}");
            return sqlQuery;
        }
        public void Execute(StringBuilder str)
        {
            
        }
        public StringBuilder QueryToWhere(Expression<Func<T, bool>> expression) { return new StringBuilder(); }

        //public T ExecuteQuery(StringBuilder str) { return ; }
        private bool IsPrimaryKeyIsForeignKey()
        {
            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo propertyInfo in properties)
            {
                var attrs = Attribute.GetCustomAttributes(propertyInfo);
                bool propIsPk = false;
                bool propIsFk = false;
                foreach (var attr in attrs)
                {
                    if (!propIsFk && attr is ForeignKeyAttribute)
                        propIsFk = true;

                    else if (!propIsPk && attr is PrimaryKeyAttribute)
                        propIsPk = true;
                }

                if (propIsPk && propIsFk)
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
        public string GetTableName()
        {
            return ((TableNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(TableNameAttribute))).TableName;
        }

    }
}
