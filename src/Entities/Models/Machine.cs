using System;

namespace Entities.Models
{
    public class Machine
    {
        public Guid MachineId { get; set; }
        public string Identifier { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        
    }
}