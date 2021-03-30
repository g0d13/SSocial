using System;
using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class RequestDto
    {
        public Guid RequestId { get; set; }
        public string Description { get; set; }
        
        [Required]
        public Scale Priority { get; set; }
        
        [Required]
        public Guid Supervisor { get; set; }
        
        [Required]
        public Guid Machine { get; set; }
        
        [Required]
        public string ProblemCode { get; set; }
        
        [Required]
        public Guid Log { get; set; }
        
        public DateTime CreatedAt { get; set; }

    }
}