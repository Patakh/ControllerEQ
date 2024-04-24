using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class TerminalDto
    {
        public long Id { get; set; }
        public string Ip { get; set; }
        public string Name { get; set; }
        public OfficeDto Office { get; set; } = new OfficeDto();
        public TerminalButtonSettings ButtonSettings { get; set; } = new TerminalButtonSettings();
    }
}
