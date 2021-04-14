using System;

namespace Entities.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        public string Content { get; set; }
        
        public bool Seen { get; set; }
        
        public DateTime Send { get; set; } = DateTime.Now;
        public DateTime Read { get; set; }
        
        public Guid FromId { get; set; }
        public User From { get; set; }
        
        public Guid ToId { get; set; }
        public User To { get; set; }
        
    }
}