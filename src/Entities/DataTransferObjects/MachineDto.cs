using System;

namespace Entities.DataTransferObjects
{
    public class MachineDto
    {
        public Guid MachineId { get; set; }
        
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }

        public Guid Category { get; set; }
    }
    public class CreateMachineDto
    {
        public Guid MachineId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }

        public Guid Log { get; set; }
    }
}