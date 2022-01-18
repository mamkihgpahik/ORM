using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    internal class Field
    {
        public Field()
        {
            this.ID = Guid.NewGuid();
        }

        public Guid ID { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}
