using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SSocial.Data;

namespace SSocial.Models
{
    public class Log
    {
        public Guid LogId { get; set; }

        public string Name { get; set; }
        
        public string Details { get; set; }
        
        
        public Guid MechanicId { get; set; }
        public virtual ApplicationUser Mechanic { get; set; }

        //For category Entity
        public ICollection<Category> Categories { get; set; }
        

    }
}