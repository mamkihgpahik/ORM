using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Attributes
{
    public class ForeignKeyAttribute : Attribute
    {
        public string ToTableName { get; set; }

        public ForeignKeyAttribute(string ToTableName)
        {
            this.ToTableName = ToTableName;
        }
    }
}
