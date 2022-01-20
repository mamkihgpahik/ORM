using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    public class Point
    {
        public Guid ID { get; set; }   
        public int X { get; set; }
        public int Y { get; set; }
        public Point()
        {
            this.ID = Guid.NewGuid();
        }
    }
}
