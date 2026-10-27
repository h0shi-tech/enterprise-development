using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище справочника пород
/// </summary>
public interface IBreedRepository
{
    IReadOnlyList<Breed> GetAll();

    Breed? FindById(int id);

    IReadOnlyList<Breed> GetBySpecies(AnimalSpecies species);

    Breed Add(string name, AnimalSpecies species);

    Breed Update(int id, string name, AnimalSpecies species);

    void Remove(int id);
}
