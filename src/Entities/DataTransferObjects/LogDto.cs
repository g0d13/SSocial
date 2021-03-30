using System;
using System.Collections.Generic;

namespace Entities.DataTransferObjects
{
    public class LogDto
    {
        public Guid LogId { get; set; }
        public string Name { get; set; }
        
        public string Details { get; set; }
        
        public ICollection<Guid> Categories { get; set; }
        public Guid Mechanic { get; set; }
    }
}