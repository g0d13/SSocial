using System;
using Microsoft.AspNetCore.Identity;
using SSocial.Utils;

namespace SSocial.Models
{
    public class Repair
    {
        public Guid RepairId { get; set; }
        public bool IsFixed { get; set; }
        public string Details { get; set; }
        public IdentityUser Mechanic { get; set; }
        public Scale Severity { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
    }
}