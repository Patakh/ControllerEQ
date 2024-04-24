using System;
using System.Collections.Generic;

namespace СontrollerEQ.Model.Data.Context;

public partial class DjangoQOrmq
{
    public int Id { get; set; }

    public string Key { get; set; } = null!;

    public string Payload { get; set; } = null!;

    public DateTime? Lock { get; set; }
}
