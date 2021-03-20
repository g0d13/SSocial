using System;
using System.Collections;
using System.Collections.Generic;

namespace SSocial.Models
{
    public class Machine
    {
        public Guid MachineId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        
        public ICollection<Category> Categories { get; set; } 

        public Log Log { get; set; }
    }
}