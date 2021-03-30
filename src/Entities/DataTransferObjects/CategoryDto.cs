using System;

namespace Entities.DataTransferObjects
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
    }
}