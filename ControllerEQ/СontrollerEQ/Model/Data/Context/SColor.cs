using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Цвета
/// </summary>
public partial class SColor
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Значение цвета
    /// </summary>
    public string ColorValue { get; set; } = null!;

    /// <summary>
    /// Описание
    /// </summary>
    public string ColorText { get; set; } = null!;
}
