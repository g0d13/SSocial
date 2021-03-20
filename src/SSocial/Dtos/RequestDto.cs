using System;
using SSocial.Utils;

namespace SSocial.Dtos
{
    public class RequestDto
    {
        public Guid RequestId { get; set; }
        public string Description { get; set; }
        public Scale Priority { get; set; }
        public Guid Supervisor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }

    }
}