using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище ветеринаров
/// </summary>
public interface IVeterinarianRepository
{
    IReadOnlyList<Veterinarian> GetAll();

    Veterinarian? FindById(int id);

    IReadOnlyList<Veterinarian> GetBySpecies(AnimalSpecies species);

    IReadOnlyList<Veterinarian> GetBySpecialization(int specializationId);

    Veterinarian Add(
        string passportNumber,
        string fullName,
        int specializationId,
        int birthYear,
        int experienceYears);

    Veterinarian Update(
        int id,
        string passportNumber,
        string fullName,
        int specializationId,
        int birthYear,
        int experienceYears);

    void Remove(int id);
}
