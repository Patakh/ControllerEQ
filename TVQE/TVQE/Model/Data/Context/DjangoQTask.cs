using System;
using System.Collections.Generic;

namespace TVQE.Model.Data.Context;

public partial class DjangoQTask
{
    public string Name { get; set; } = null!;

    public string Func { get; set; } = null!;

    public string? Hook { get; set; }

    public string? Args { get; set; }

    public string? Kwargs { get; set; }

    public string? Result { get; set; }

    public DateTime Started { get; set; }

    public DateTime Stopped { get; set; }

    public bool Success { get; set; }

    public string Id { get; set; } = null!;

    public string? Group { get; set; }

    public int AttemptCount { get; set; }
}
