using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class TiketSaveRequestDto
    {
        public long OfficeId { get; set; }
        public long OfficeTerminalId { get; set; }
        public long ServiceId { get; set; }
        public long? PriorityId { get; set; }
        public long? PrerecordId { get; set; }
    }
}
