using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SSocial.Models
{
    public class Log
    {
        public Guid LogId { get; set; }

        public string Name { get; set; }
        
        public ICollection<Machine> Machines { get; set; }
        
        public IdentityUser Mechanic { get; set; }
    }
}