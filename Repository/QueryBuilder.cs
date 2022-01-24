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

            if (IsPrimaryKeyIsForeignKey())
            {
                sqlQuery.Append($"INSERT INTO {DataBase}.dbo.{GetTableName()} (");

                for (int i = 0; i < properties.Length; i++)
                {
                    sqlQuery.Append(properties[i].Name);

                    if (i != properties.Length - 1)
                        sqlQuery.Append(",");
                }

                sqlQuery.Append(")\n");
                sqlQuery.Append("VALUES(");

                for (int i = 0; i < properties.Length; i++)
                {
                    if (IsNumber(properties[i].GetValue(entity)))
                        sqlQuery.Append(properties[i].GetValue(entity));
                    else
                        sqlQuery.Append($"\'{properties[i].GetValue(entity)}\'");

                    if (i != properties.Length - 1)
                        sqlQuery.Append(",");
                }

                sqlQuery.Append(")");

                return sqlQuery;
            }

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

        public StringBuilder QueryToUpdate(T entity) 
        {
            var properties = entity.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            StringBuilder sqlQuery = new StringBuilder($"UPDATE {DataBase}.dbo.{GetTableName()}\nSET ");

            for (int i = 1; i < properties.Length; i++)
            {
                if (IsNumber(properties[i].GetValue(entity)))
                    sqlQuery.Append($"{properties[i].Name} = {properties[i].GetValue(entity)}");

                if (i != properties.Length - 1)
                    sqlQuery.Append(", ");
                else
                    sqlQuery.Append("\n");
            }

            sqlQuery.Append($"WHERE {properties[0].Name} = {properties[0].GetValue(entity)}");

            return sqlQuery;
        }

        public StringBuilder QueryToDeleteById(Guid id) 
        {
            StringBuilder sqlQuery = new StringBuilder();
            return sqlQuery.Append($"DELETE FROM {DataBase}.dbo.{GetTableName()} WHERE Id = {id}");
        }

        public StringBuilder QueryToGetById(Guid id)
        {
            StringBuilder sql = new StringBuilder();
            //if (IsPrimaryKeyIsForeignKey())
            //    return GenerateJoinPartQueryForRelation_1_to_1() + $"WHERE {GetTableName()}.{GetPrimaryKeyName()} = {id}";

            return sql.Append($"SELECT TOP(1) * FROM {DataBase}.dbo.{GetTableName()} WHERE Id = {id}");      
        }

        public StringBuilder QueryToGetAll() 
        {
            //if (IsPrimaryKeyIsForeignKey())
            //{
            //    return GenerateJoinPartQueryForRelation_1_to_1();
            //}
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append($"SELECT * FROM {DataBase}.dbo.{GetTableName()}");
            return sqlQuery;
        }

        public StringBuilder GenerateSqlQueryToWhere(Expression<Func<T, bool>> expression)
        {
            List<BinaryExpression> orderedBinaryExpressions = new List<BinaryExpression>();

            StringBuilder sqlQuery = new StringBuilder();

            if (IsPrimaryKeyIsForeignKey())
            {
                sqlQuery.Append(GenerateJoinPartQueryForRelation_1_to_1());
                sqlQuery.Append("WHERE ");

                FillBinaryList(expression.Body as BinaryExpression, orderedBinaryExpressions);

                foreach (var binExp in orderedBinaryExpressions)
                {
                    if (binExp.Left.NodeType == ExpressionType.Constant || binExp.Left.NodeType == ExpressionType.MemberAccess &&
                       binExp.Right.NodeType == ExpressionType.Constant || binExp.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        sqlQuery.Append($" {GetTableName()}.{typeof(T).GetProperty(binExp.Left.ToString().Substring(binExp.Left.ToString().IndexOf('.') + 1)).Name} {nodeTypesToSql[binExp.NodeType]} {binExp.Right.ToString().Replace('\"', '\'')} ");
                    }
                    else
                    {
                        sqlQuery.Append($" {nodeTypesToSql[binExp.NodeType]} ");
                    }
                }

                return sqlQuery;
            }

            sqlQuery.Append($"SELECT * FROM {GetTableName()}\n");
            sqlQuery.Append("WHERE ");

            FillBinaryList(expression.Body as BinaryExpression, orderedBinaryExpressions);

            foreach (var binExp in orderedBinaryExpressions)
            {
                if (binExp.Left.NodeType == ExpressionType.Constant || binExp.Left.NodeType == ExpressionType.MemberAccess &&
                   binExp.Right.NodeType == ExpressionType.Constant || binExp.Right.NodeType == ExpressionType.MemberAccess)
                {
                    if (binExp.Right.NodeType == ExpressionType.MemberAccess)
                    {
                        var f = Expression.Lambda(binExp.Right).Compile();
                        var value = f.DynamicInvoke();

                        sqlQuery.Append($" {typeof(T).GetProperty(binExp.Left.ToString().Substring(binExp.Left.ToString().IndexOf('.') + 1)).Name} {nodeTypesToSql[binExp.NodeType]} {value} ");
                    }
                    else
                    {
                        sqlQuery.Append($" {typeof(T).GetProperty(binExp.Left.ToString().Substring(binExp.Left.ToString().IndexOf('.') + 1)).Name} {nodeTypesToSql[binExp.NodeType]} {binExp.Right.ToString().Replace('\"', '\'')} ");
                    }
                }
                else
                {
                    sqlQuery.Append($" {nodeTypesToSql[binExp.NodeType]} ");
                }
            }

            return sqlQuery;
        }
        static BinaryExpression FillBinaryList(BinaryExpression binExp, List<BinaryExpression> orderedBinaryExpressions)
        {
            if (binExp?.Left.NodeType != ExpressionType.Constant && binExp?.Left.NodeType != ExpressionType.MemberAccess)
            {
                FillBinaryList(binExp.Left as BinaryExpression, orderedBinaryExpressions);
                orderedBinaryExpressions.Add(binExp);
            }
            if (binExp?.Right.NodeType != ExpressionType.Constant && binExp?.Right.NodeType != ExpressionType.MemberAccess)
            {
                FillBinaryList(binExp.Right as BinaryExpression, orderedBinaryExpressions);
            }

            if (!orderedBinaryExpressions.Any(b => b == binExp))
                orderedBinaryExpressions.Add(binExp);
            return binExp;
        }



        private string GenerateJoinPartQueryForRelation_1_to_1()
        {
            string PK_NameOfFatherTable = String.Empty;
            string PK_NameOfDoutherTable = String.Empty;
            string mainTableName = String.Empty;
            StringBuilder sqlQuery = new StringBuilder("");

            var properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var attrs = Attribute.GetCustomAttributes(prop);
                foreach (var attr in attrs)
                {
                    if (attr is ForeignKeyAttribute)
                    {
                        mainTableName = (attr as ForeignKeyAttribute).ToTableName;
                        PK_NameOfDoutherTable = prop.Name;
                        break;
                    }
                }
            }

            if (PK_NameOfDoutherTable == String.Empty)
                throw new Exception($"Не указан вторичный ключ сущности {typeof(T).Name}");

            PropertyInfo[] propertiesFromMainTable = typeof(T).BaseType.GetProperties();

            sqlQuery.Append($"SELECT ");

            foreach (PropertyInfo prop in properties)
            {
                sqlQuery.Append(GetTableName() + "." + prop.Name + ", ");
            }

            for (int i = 0; i < propertiesFromMainTable.Length; i++)
            {
                if (!propertiesFromMainTable[i].CustomAttributes.Any(x => x.AttributeType == typeof(PrimaryKeyAttribute)))
                {
                    sqlQuery.Append(mainTableName + "." + propertiesFromMainTable[i].Name);

                    if (i != propertiesFromMainTable.Length - 1)
                        sqlQuery.Append(", ");
                    else
                        sqlQuery.Append(" ");
                }
                else
                {
                    PK_NameOfFatherTable = propertiesFromMainTable[i].Name;
                }
            }

            sqlQuery.Append($"FROM {GetTableName()}\n");
            sqlQuery.Append($"INNER JOIN {mainTableName}\n");
            sqlQuery.Append($"ON {GetTableName()}.{PK_NameOfDoutherTable} = {mainTableName}.{PK_NameOfFatherTable}\n");

            return sqlQuery.ToString();
        }

        
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
