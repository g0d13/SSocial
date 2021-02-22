using System;
using System.Collections.Generic;

namespace SSocial.Dtos
{
    public class CreateLogDto
    {
        public Guid LogId { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> Machines { get; set; }
        public string Mechanic { get; set; }
    }

    public class GetLogDto
    {
        public Guid LogId { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> Machines { get; set; }
        public string Mechanic { get; set; }
    }
}