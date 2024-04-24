using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.Models.DTO
{
    public class TerminalButtonSettings
    {
        public TerminalButtonSettings() { }
        public TerminalButtonSettings(int row, int column) { 
            RowCount = row;
            ColCount = column;
        }
        public long RowCount { get; set; }
        public long ColCount { get; set; }
    }
}
