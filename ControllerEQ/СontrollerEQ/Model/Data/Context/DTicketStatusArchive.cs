﻿using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Статусы талонов в архиве
/// </summary>
public partial class DTicketStatusArchive
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Талон
    /// </summary>
    public long DTicketArchiveId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public long SStatusId { get; set; }

    /// <summary>
    /// Сотрудник
    /// </summary>
    public long? SEmployeeId { get; set; }

    /// <summary>
    /// Окно
    /// </summary>
    public long? SOfficeWindowId { get; set; }

    /// <summary>
    /// Окно куда перенаправили
    /// </summary>
    public long? SOfficeWindowIdTransferred { get; set; }

    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly? DateAdd { get; set; }

    /// <summary>
    ///  Время
    /// </summary>
    public TimeOnly? TimeAdd { get; set; }

    /// <summary>
    /// Кто добавил
    /// </summary>
    public string? EmployeeNameAdd { get; set; }

    public virtual SStatus SStatus { get; set; } = null!;
}
