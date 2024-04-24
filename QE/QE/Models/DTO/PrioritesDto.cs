using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class PrioritesDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Comment { get; set; }
        public long SortId { get; set;}
    }
}
