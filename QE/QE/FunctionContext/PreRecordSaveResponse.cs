using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QE.FunctionContext
{
    public class PreRecordSave
    {
        [Column("out_code_prerecord")]
        public long Number { get; set; }
    }
}
