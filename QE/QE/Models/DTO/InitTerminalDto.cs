using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class InitTerminalDto
    {
        public TerminalDto terminalDto { get; set; }
        public List<SchedulesDto> schedulesDto { get; set; } = new List<SchedulesDto>();
        public ColorDto colorDto { get; set; }
        public Error? Error { get; set; }
    }

    public class Error
    {
        public int Type { get; set; }
        public string Message { get; set; }
    }
}
