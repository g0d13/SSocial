using System;
using System.Collections;
using System.Collections.Generic;

namespace SSocial.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<Machine> Machines { get; set; }
    }
}