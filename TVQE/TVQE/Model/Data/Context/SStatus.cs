using System;
using System.Collections.Generic;

namespace TVQE.Model.Data.Context;

/// <summary>
/// Справочник статусов талонов
/// </summary>
public partial class SStatus
{
    /// <summary>
    ///  Наименование
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string StatusName { get; set; } = null!;

    public virtual ICollection<DTicketArchive> DTicketArchives { get; set; } = new List<DTicketArchive>();

    public virtual ICollection<DTicketStatusArchive> DTicketStatusArchives { get; set; } = new List<DTicketStatusArchive>();

    public virtual ICollection<DTicketStatus> DTicketStatuses { get; set; } = new List<DTicketStatus>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();
}
