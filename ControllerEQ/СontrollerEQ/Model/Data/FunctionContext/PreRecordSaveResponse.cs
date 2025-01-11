using System.ComponentModel.DataAnnotations.Schema;

namespace ControllerEQ.Model.Data.Context;

public class PreRecordSave
{
    [Column("out_code_prerecord")]
    public long Number { get; set; }
}
