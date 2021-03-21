using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using SSocial.Utils;

namespace SSocial.Dtos
{
    public class RepairDto
    {
        public Guid RepairId { get; set; }
        
        public bool IsFixed { get; set; }
        
        public string Details { get; set; }
        [Required]
        
        public Guid Log { get; set; }
        public Guid Mechanic { get; set; }
        
        public Scale Severity { get; set; }
        
        public DateTime ArrivalTime { get; set; }
        
        public DateTime DepartureTime { get; set; }

    }
}