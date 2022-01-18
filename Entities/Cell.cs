using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    internal class Cell
    {
        public Guid ID { get; set; }
        public Guid ShipId { get; set; }
        public Guid PointId { get; set; }
        public Guid QuadrantId { get; set; }
        public Guid FieldId { get; set; }
        public Cell()
        {
            this.ID = Guid.NewGuid();
        }
    }
}
