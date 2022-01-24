using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class QueryResult
    {
        public Status status { get; set; }
        public DataSet Result { get; set; }
        public int? InsertedId { get; set; }
        public Exception exception { get; set; }
    }
}
