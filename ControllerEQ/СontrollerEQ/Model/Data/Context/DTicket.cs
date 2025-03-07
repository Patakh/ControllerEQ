﻿using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Талоны в очереди
/// </summary>
public partial class DTicket
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
    /// Терминал
    /// </summary>
    public long? SOfficeTerminalId { get; set; }

    /// <summary>
    /// Услуга
    /// </summary>
    public long SServiceId { get; set; }

    /// <summary>
    /// Префикс
    /// </summary>
    public string ServicePrefix { get; set; } = null!;

    /// <summary>
    /// Приоритет заявителя
    /// </summary>
    public long? SPriorityId { get; set; }

    /// <summary>
    /// Номер талона
    /// </summary>
    public long TicketNumber { get; set; }

    /// <summary>
    /// Полный номер талона
    /// </summary>
    public string TicketNumberFull { get; set; } = null!;

    /// <summary>
    /// Связь с пред записью
    /// </summary>
    public long? DTicketPrerecordId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public long SStatusId { get; set; }

    /// <summary>
    /// Окно
    /// </summary>
    public long? SOfficeWindowId { get; set; }

    /// <summary>
    /// Дата постановки в очередь
    /// </summary>
    public DateOnly DateRegistration { get; set; }

    /// <summary>
    /// Время постановки в очередь 
    /// </summary>
    public TimeOnly TimeRegistration { get; set; }

    /// <summary>
    /// Время окончания обслуживания
    /// </summary>
    public TimeOnly? TimeStopService { get; set; }

    /// <summary>
    /// Время вызова
    /// </summary>
    public TimeOnly? TimeCall { get; set; }

    /// <summary>
    /// Время начала обслужевания
    /// </summary>
    public TimeOnly? TimeStartService { get; set; }

    /// <summary>
    /// Время ожидания
    /// </summary>
    public TimeOnly? TimeWaiting { get; set; }

    /// <summary>
    /// Время обслуживания
    /// </summary>
    public TimeOnly? TimeService { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public long? SEmployeeId { get; set; }

    public virtual DTicketPrerecord? DTicketPrerecord { get; set; }

    public virtual ICollection<DTicketStatus> DTicketStatuses { get; set; } = new List<DTicketStatus>();

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual SOfficeTerminal? SOfficeTerminal { get; set; }

    public virtual SOfficeWindow? SOfficeWindow { get; set; }

    public virtual SPriority? SPriority { get; set; }

    public virtual SService SService { get; set; } = null!;

    public virtual SStatus SStatus { get; set; } = null!;
}
