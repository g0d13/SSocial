using System;

namespace Entities.Models
{
    public class Repair
    {
        public Guid RepairId { get; set; }
        public bool IsFixed { get; set; }
        public string Details { get; set; }
        public Scale Severity { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        

        public Guid LogId { get; set; }
        public Log Log { get; set; }
        

        public Guid MechanicId { get; set; }
        public User Mechanic { get; set; }
    }
}