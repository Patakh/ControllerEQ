﻿using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

public partial class DjangoAdminLog
{
    public int Id { get; set; }

    public DateTime ActionTime { get; set; }

    public string? ObjectId { get; set; }

    public string ObjectRepr { get; set; } = null!;

    public short ActionFlag { get; set; }

    public string ChangeMessage { get; set; } = null!;

    public int? ContentTypeId { get; set; }

    public long UserId { get; set; }

    public virtual DjangoContentType? ContentType { get; set; }

    public virtual User User { get; set; } = null!;
}
