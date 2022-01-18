using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    internal class Ship
    {
        public Guid ID { get; set; }
        public int Size { get; set; }
        public int Radius { get; set; }
        public Guid ShipTypeId { get; set; }
        public Guid DirectionId { get; set; }
        public Ship()
        {
            this.ID = Guid.NewGuid();
        }
    }
}
