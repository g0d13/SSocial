using System;

namespace Entities.Models
{
    public class Record
    {
        public Guid RecordId { get; set; }
        
        public Guid RequestId { get; set; }
        public Request Request { get; set; }

        public Guid RepairId { get; set; }
        public Repair Repair { get; set; }

        public Guid MachineId { get; set; }
        public Machine Machine { get; set; }
        
        public Guid LogId { get; set; }
        public Log Log { get; set; }
    }
}