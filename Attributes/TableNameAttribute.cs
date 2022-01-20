using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Attributes
{
    public class TableNameAttribute : Attribute
    {
        public string TableName { get; set; }
        public TableNameAttribute(string tableName)
        {
            this.TableName = tableName;
        }
    }
}
