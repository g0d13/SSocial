using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public string Description { get; set; }
        public Scale Priority { get; set; }
        
        public string ProblemCode { get; set; }
        
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        public ICollection<Repair> Repairs { get; set; }
        
        
        public Guid SupervisorId { get; set; }
        public User Supervisor { get; set; }
        

        public Guid MachineId { get; set; }
        public Machine Machine { get; set; }
        

        public Guid LogId { get; set; }
        public Log Log { get; set; }
        
    }
    
}