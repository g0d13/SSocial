using System;
using System.Collections.Generic;

namespace SSocial.Dtos
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }
}