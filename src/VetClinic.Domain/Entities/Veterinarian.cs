namespace VetClinic.Domain.Entities;

/// <summary>
/// Ветеринарный врач
/// </summary>
public class Veterinarian
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Номер паспорта
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// ФИО врача
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Специализация врача
    /// </summary>
    public required Specialization Specialization { get; set; }

    /// <summary>
    /// Год рождения
    /// </summary>
    public int BirthYear { get; set; }

    /// <summary>
    /// Стаж работы в годах
    /// </summary>
    public int ExperienceYears { get; set; }
}
