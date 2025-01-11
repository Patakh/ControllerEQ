using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

public partial class DjangoQSchedule
{
    public int Id { get; set; }

    public string Func { get; set; } = null!;

    public string? Hook { get; set; }

    public string? Args { get; set; }

    public string? Kwargs { get; set; }

    public string ScheduleType { get; set; } = null!;

    public int Repeats { get; set; }

    public DateTime? NextRun { get; set; }

    public string? Task { get; set; }

    public string? Name { get; set; }

    public short? Minutes { get; set; }

    public string? Cron { get; set; }

    public string? Cluster { get; set; }
}
