﻿using System;
using System.Collections.Generic;

namespace TVQE.Model.Data.Context;

public partial class DjangoSession
{
    public string SessionKey { get; set; } = null!;

    public string SessionData { get; set; } = null!;

    public DateTime ExpireDate { get; set; }
}