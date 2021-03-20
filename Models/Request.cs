using System;
using Microsoft.AspNetCore.Identity;
using SSocial.Data;
using SSocial.Utils;

namespace SSocial.Models
{
    public class Request
    {
        public Guid RequestId { get; set; }
        public string Description { get; set; }
        public Scale Priority { get; set; }
        public virtual ApplicationUser Supervisor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
    }
    
}