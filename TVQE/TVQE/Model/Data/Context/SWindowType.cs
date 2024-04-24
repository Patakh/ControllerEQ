using System;
using System.Collections.Generic;

namespace TVQE.Model.Data.Context;

/// <summary>
/// Тип окна
/// </summary>
public partial class SWindowType
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование типа
    /// </summary>
    public string TypeName { get; set; } = null!;

    public virtual ICollection<SOfficeWindow> SOfficeWindows { get; set; } = new List<SOfficeWindow>();
}
