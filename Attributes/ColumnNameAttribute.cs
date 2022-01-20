using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Attributes
{
    public class ColumnNameAttribute : Attribute
    {
        public string ColumnName { get; set; }
        public ColumnNameAttribute(string ColumnName)
        {
            this.ColumnName = ColumnName;
        }
    }
}
