using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Entities;

/// <summary>
/// Питомец, обслуживаемый в клинике
/// </summary>
public class Pet
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Кличка
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Порода питомца
    /// </summary>
    public required Breed Breed { get; set; }

    /// <summary>
    /// Владелец питомца
    /// </summary>
    public required Owner Owner { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateOnly BirthDate { get; set; }

    /// <summary>
    /// Вес в килограммах
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Биологический вид животного; определяется породой
    /// </summary>
    public AnimalSpecies Species => Breed.Species;
}
