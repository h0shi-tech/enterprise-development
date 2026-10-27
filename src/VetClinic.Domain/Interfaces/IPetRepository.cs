using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище питомцев
/// </summary>
public interface IPetRepository
{
    IReadOnlyList<Pet> GetAll();

    Pet? FindById(int id);

    IReadOnlyList<Pet> GetByOwner(int ownerId);

    IReadOnlyList<Pet> GetByVeterinarian(int veterinarianId);

    Pet Add(string name, int breedId, int ownerId, DateOnly birthDate, double weight);

    Pet Update(int id, string name, int breedId, int ownerId, DateOnly birthDate, double weight);

    void Remove(int id);
}
