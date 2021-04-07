using System;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class RecordDto
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