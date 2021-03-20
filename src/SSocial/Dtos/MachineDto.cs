using System;

namespace SSocial.Dtos
{
    public class GetMachineDto
    {
        public Guid MachineId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
    }
    public class CreateMachineDto
    {
        public Guid MachineId { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }

        public Guid Log { get; set; }
    }
}