using VetClinic.Domain.Entities;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class OwnerRepository(InMemoryClinicStore store) : IOwnerRepository
{
    public IReadOnlyList<Owner> GetAll()
    {
        lock (store.Sync)
        {
            return store.Owners.ToArray();
        }
    }

    public Owner? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Owners.FirstOrDefault(owner => owner.Id == id);
        }
    }

    public IReadOnlyList<Owner> GetWithSeveralPets()
    {
        lock (store.Sync)
        {
            return store.Owners
                .Where(owner => owner.Pets.Count > 1)
                .OrderBy(owner => owner.FullName)
                .ToArray();
        }
    }

    public Owner Add(string fullName, string phoneNumber, string? address)
    {
        lock (store.Sync)
        {
            var owner = new Owner
            {
                Id = store.TakeOwnerId(),
                FullName = fullName,
                PhoneNumber = phoneNumber,
                Address = address
            };
            store.Owners.Add(owner);
            return owner;
        }
    }

    public Owner Update(int id, string fullName, string phoneNumber, string? address)
    {
        lock (store.Sync)
        {
            var owner = store.Owners.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("владелец", id);
            owner.FullName = fullName;
            owner.PhoneNumber = phoneNumber;
            owner.Address = address;
            return owner;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var owner = store.Owners.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("владелец", id);

            if (owner.Pets.Count > 0)
            {
                throw new EntityInUseException($"У владельца «{owner.FullName}» есть питомцы, удаление невозможно.");
            }

            store.Owners.Remove(owner);
        }
    }
}
