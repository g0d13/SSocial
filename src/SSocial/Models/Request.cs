using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SSocial.Data;
using SSocial.Utils;

namespace SSocial.Models
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
        public ApplicationUser Supervisor { get; set; }
        

        public Guid MachineId { get; set; }
        public Machine Machine { get; set; }
        

        public Guid LogId { get; set; }
        public Log Log { get; set; }
        
    }
    
}