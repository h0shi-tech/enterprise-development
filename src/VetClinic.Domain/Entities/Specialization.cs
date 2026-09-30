using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Entities;

/// <summary>
/// Специализация ветеринара — справочник врачебных профилей клиники
/// </summary>
public class Specialization
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название профиля, например «Хирург»
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Биологический вид животных, который принимает врач с этой специализацией
    /// </summary>
    public AnimalSpecies Species { get; set; }
}
