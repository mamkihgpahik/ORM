using ORM.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    [TableName("Directions")]
    public class Direction
    {
        [PrimaryKey]
        public int ID { get; set; }
        
        public string Name { get; set; }
        public Direction(int id, string name)
        {
            this.ID = id;   
            this.Name = name;
        }
    }
}
