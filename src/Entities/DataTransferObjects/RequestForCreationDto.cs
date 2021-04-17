using System;
using System.ComponentModel.DataAnnotations;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class RequestForCreationDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        [Required] public Scale Priority { get; set; }

        [Required] public Guid Supervisor { get; set; }

        [Required] public Guid Machine { get; set; }

        [Required] public string ProblemCode { get; set; }

        [Required] public Guid Log { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class RequestDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public Scale Priority { get; set; }

        public Guid SupervisorId { get; set; }

        public Guid MachineId { get; set; }

        public string ProblemCode { get; set; }

        public Guid LogId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}