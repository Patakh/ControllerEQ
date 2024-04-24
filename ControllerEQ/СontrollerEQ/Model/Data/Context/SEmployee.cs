using System;
using System.Collections.Generic;

namespace СontrollerEQ.Model.Data.Context;

/// <summary>
/// Справочник сотрудников
/// </summary>
public partial class SEmployee
{
    /// <summary>
    /// Ключ
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Офис
    /// </summary>
    public long SOfficeId { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    public long SRoleId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// ФИО
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    ///  Кто добавил
    /// </summary>
    public string EmployeeNameAdd { get; set; } = null!;

    /// <summary>
    /// Дата и время добавления
    /// </summary>
    public DateTime? DateAdd { get; set; }

    public virtual ICollection<DTicketPrerecord> DTicketPrerecords { get; set; } = new List<DTicketPrerecord>();

    public virtual SOffice SOffice { get; set; } = null!;

    public virtual SRole SRole { get; set; } = null!;
}
