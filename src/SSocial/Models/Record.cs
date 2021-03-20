using System;

namespace SSocial.Models
{
    public class Record
    {
        public Guid RecordId { get; set; }
        public Request Request { get; set; }
        public Repair Repair { get; set; }
        public Machine Machine { get; set; }
        public Log Log { get; set; }
    }
}