using System.ComponentModel.DataAnnotations.Schema;

namespace QE.FunctionContext
{
    public class DTicketSaveResponse
    {
        [Column("out_ticket_number_full")]
        public string TicketNumberFull { get; set; }
        [Column("out_count")]
        public long Count { get; set; }
    }
}
