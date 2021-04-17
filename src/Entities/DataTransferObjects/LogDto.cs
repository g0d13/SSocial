using System;
using System.Collections.Generic;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class LogDto : LogBaseDto
    {
        
        public ICollection<CategoryDto> Categories { get; set; }
        
        public UserDetails Mechanic { get; set; }
    }

    public class LogBaseDto
    {
        public Guid LogId { get; set; }
        public string Name { get; set; }
        
        public string Details { get; set; }
    }
    public class LogForCreationDto : LogBaseDto
    {
        public ICollection<Guid> Categories { get; set; }
        public Guid Mechanic { get; set; }
    }
}