using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Log
    {
        public Guid LogId { get; set; }

        public string Name { get; set; }
        
        public string Details { get; set; }
        
        
        public Guid MechanicId { get; set; }
        public virtual User Mechanic { get; set; }

        //For category Entity
        public ICollection<Category> Categories { get; set; }
        

    }
}