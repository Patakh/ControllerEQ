using System.ComponentModel.DataAnnotations.Schema;

namespace ControllerEQ.Model.Data.Context;

public class DTicketSaveResponse
{
    [Column("out_ticket_number_full")]
    public string TicketNumberFull { get; set; }
    [Column("out_count")]
    public long Count { get; set; }
}
