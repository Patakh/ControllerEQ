using System;
using System.Collections.Generic;

namespace ControllerEQ.Model.Data.Context;

/// <summary>
/// Голос
/// </summary>
public partial class SVoice
{
    /// <summary>
    /// Первичный ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string VoiceName { get; set; } = null!;

    /// <summary>
    /// Файл
    /// </summary>
    public byte[] File { get; set; } = null!;
}
