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
        public QueryBuilder()
        {
            nodeTypesToSql.Add(ExpressionType.GreaterThan, ">");
            nodeTypesToSql.Add(ExpressionType.LessThan, "<");
            nodeTypesToSql.Add(ExpressionType.OrElse, "OR");
            nodeTypesToSql.Add(ExpressionType.Equal, "=");
            nodeTypesToSql.Add(ExpressionType.AndAlso, "AND");
            nodeTypesToSql.Add(ExpressionType.GreaterThanOrEqual, ">=");
            nodeTypesToSql.Add(ExpressionType.LessThanOrEqual, "<=");
        }
        public StringBuilder QueryToInsert(T entity) { return new StringBuilder(); }
        public StringBuilder QueryToUpdate(T entity) { return new StringBuilder(); }
        public StringBuilder QueryToDeleteById(Guid id) { return new StringBuilder(); }
        public StringBuilder QueryToGetById(Guid id) { return new StringBuilder(); }
        public StringBuilder QueryToGetAll() { return new StringBuilder(); }
        public StringBuilder QueryToWhere(Expression<Func<T, bool>> expression) { return new StringBuilder(); }


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
