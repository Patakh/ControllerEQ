namespace СontrollerEQ.Model;

public enum Status
{
    /// <summary>
    /// Новый
    /// </summary>
    New = 1,

    /// <summary>
    /// Вызов
    /// </summary>
    Call = 2,

    /// <summary>
    /// Обслуживается
    /// </summary>
    Serviced = 3,

    /// <summary>
    /// 
    /// </summary>
    Transferred = 4,

    /// <summary>
    /// Передан
    /// </summary>
    Defer = 5,

    /// <summary>
    /// Обслуживание завершено
    /// </summary>
    Completed = 6,

    /// <summary>
    /// Отмена
    /// </summary>
    Cancel = 7,

    /// <summary>
    /// Не явился
    /// </summary>
    NeverShowed = 8,
}
