using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Attributes
{
    public class OneToManyAttribute : Attribute
    {
        public string TableName { get; set; }
        public OneToManyAttribute(string TableName)
        {
            this.TableName = TableName;
        }
    }
}
