using System;
using Microsoft.AspNetCore.Identity;
using SSocial.Utils;

namespace SSocial.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public string Description { get; set; }
        public Scale Priority { get; set; }
        public IdentityUser Supervisor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
    }
}