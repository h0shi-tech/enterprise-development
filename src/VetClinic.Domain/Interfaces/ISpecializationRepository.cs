using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище справочника специализаций
/// </summary>
public interface ISpecializationRepository
{
    IReadOnlyList<Specialization> GetAll();

    Specialization? FindById(int id);

    IReadOnlyList<Specialization> GetBySpecies(AnimalSpecies species);

    Specialization Add(string name, AnimalSpecies species);

    Specialization Update(int id, string name, AnimalSpecies species);

    void Remove(int id);
}
