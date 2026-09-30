namespace VetClinic.Domain.Entities;

/// <summary>
/// Запись питомца на приём к ветеринару
/// </summary>
public class Appointment
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Питомец, которого записали на приём
    /// </summary>
    public required Pet Pet { get; set; }

    /// <summary>
    /// Врач, к которому записан питомец
    /// </summary>
    public required Veterinarian Veterinarian { get; set; }

    /// <summary>
    /// Дата и время приёма
    /// </summary>
    public DateTime VisitDateTime { get; set; }

    /// <summary>
    /// Номер кабинета; может содержать буквы, поэтому хранится строкой
    /// </summary>
    public required string RoomNumber { get; set; }

    /// <summary>
    /// Признак повторного приёма
    /// </summary>
    public bool IsFollowUp { get; set; }
}
