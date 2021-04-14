using System;
using System.Collections.Generic;
using Entities.Models;

namespace SSocial.Hubs
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}