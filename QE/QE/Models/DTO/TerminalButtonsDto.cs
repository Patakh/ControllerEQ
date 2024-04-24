using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class TerminalButtonsDto
    {
        public long Id { get; set; }
        public long? ParentId { get; set; }
        public string Name { get; set; }
        public long? ButtonType { get; set; }
        public long? ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public long SortId { get; set; }
    }
}
