using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Настройки
/// </summary>
public partial class SSetting
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование параметра
    /// </summary>
    public string ParamName { get; set; } = null!;

    /// <summary>
    /// Значение параметра
    /// </summary>
    public string ParamValue { get; set; } = null!;

    /// <summary>
    /// Описание
    /// </summary>
    public string? ParamCommentt { get; set; }
}
