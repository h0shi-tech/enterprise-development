using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Entities;

/// <summary>
/// Порода животного — справочник, привязанный к биологическому виду
/// </summary>
public class Breed
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название породы
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Биологический вид, к которому относится порода
    /// </summary>
    public AnimalSpecies Species { get; set; }
}
