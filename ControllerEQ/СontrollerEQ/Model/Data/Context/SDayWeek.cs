using System;
using System.Collections.Generic;

namespace СontrollerEQ.Model.Data.Context;

/// <summary>
/// Справочник дней недели
/// </summary>
public partial class SDayWeek
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string DayName { get; set; } = null!;

    public virtual ICollection<SOfficePrerecord> SOfficePrerecords { get; set; } = new List<SOfficePrerecord>();

    public virtual ICollection<SOfficeSchedule> SOfficeSchedules { get; set; } = new List<SOfficeSchedule>();
}
