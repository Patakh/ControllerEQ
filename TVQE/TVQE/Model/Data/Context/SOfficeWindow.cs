using System;
using System.Collections.Generic;

namespace TVQE.Model.Data.Context;

/// <summary>
/// Справочник окон офисов
/// </summary>
public partial class SOfficeWindow
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Офис
    /// </summary>
    public long SOfficeId { get; set; }

    /// <summary>
    /// Наименование 
    /// </summary>
    public string WindowName { get; set; } = null!;

    /// <summary>
    /// IP Адрес
    /// </summary>
    public string? WindowIp { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Ip адрес табло оператора
    /// </summary>
    public string? ElectronicScoreboardIp { get; set; }

    /// <summary>
    /// Тип окна
    /// </summary>
    public long SWindowTypeId { get; set; }

    public virtual ICollection<DTicketArchive> DTicketArchives { get; set; } = new List<DTicketArchive>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual ICollection<SOfficeWindowService> SOfficeWindowServices { get; set; } = new List<SOfficeWindowService>();

    public virtual SWindowType SWindowType { get; set; } = null!;
}
