using VetClinic.Domain.Entities;

namespace VetClinic.Domain.Interfaces;

/// <summary>
/// Хранилище владельцев
/// </summary>
public interface IOwnerRepository
{
    IReadOnlyList<Owner> GetAll();

    Owner? FindById(int id);

    IReadOnlyList<Owner> GetWithSeveralPets();

    Owner Add(string fullName, string phoneNumber, string? address);

    Owner Update(int id, string fullName, string phoneNumber, string? address);

    void Remove(int id);
}
