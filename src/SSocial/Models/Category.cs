using System;
using System.Collections;
using System.Collections.Generic;

namespace SSocial.Models
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        
        public string Name { get; set; }

        public string Details { get; set; }
        

        public ICollection<Log> Logs { get; set; }
        
    }
}