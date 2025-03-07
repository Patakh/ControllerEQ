﻿using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Справочник услуг
/// </summary>
public partial class SService
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string ServiceName { get; set; } = null!;

    /// <summary>
    /// Префикс
    /// </summary>
    public string ServicePrefix { get; set; } = null!;

    /// <summary>
    /// Прироитет 
    /// </summary>
    public long ServicePriority { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    /// <summary>
    /// Время оказания
    /// </summary>
    public TimeOnly? TimeRendering { get; set; }

    public virtual ICollection<DTicketArchive> DTicketArchives { get; set; } = new List<DTicketArchive>();

    public virtual ICollection<DTicket> DTickets { get; set; } = new List<DTicket>();

    public virtual ICollection<SOfficeScoreboardService> SOfficeScoreboardServices { get; set; } = new List<SOfficeScoreboardService>();

    public virtual ICollection<SOfficeTerminalButton> SOfficeTerminalButtons { get; set; } = new List<SOfficeTerminalButton>();

    public virtual ICollection<SOfficeTerminalService> SOfficeTerminalServices { get; set; } = new List<SOfficeTerminalService>();

    public virtual ICollection<SOfficeWindowService> SOfficeWindowServices { get; set; } = new List<SOfficeWindowService>();
}
