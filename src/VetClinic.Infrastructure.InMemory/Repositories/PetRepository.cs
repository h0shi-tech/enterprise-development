using VetClinic.Domain.Entities;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class PetRepository(InMemoryClinicStore store) : IPetRepository
{
    public IReadOnlyList<Pet> GetAll()
    {
        lock (store.Sync)
        {
            return store.Pets.ToArray();
        }
    }

    public Pet? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Pets.FirstOrDefault(pet => pet.Id == id);
        }
    }

    public IReadOnlyList<Pet> GetByOwner(int ownerId)
    {
        lock (store.Sync)
        {
            _ = store.Owners.FirstOrDefault(owner => owner.Id == ownerId)
                ?? throw new EntityNotFoundException("владелец", ownerId);

            return store.Pets
                .Where(pet => pet.Owner.Id == ownerId)
                .OrderBy(pet => pet.Name)
                .ToArray();
        }
    }

    public IReadOnlyList<Pet> GetByVeterinarian(int veterinarianId)
    {
        lock (store.Sync)
        {
            _ = store.Veterinarians.FirstOrDefault(veterinarian => veterinarian.Id == veterinarianId)
                ?? throw new EntityNotFoundException("ветеринар", veterinarianId);

            return store.Appointments
                .Where(appointment => appointment.Veterinarian.Id == veterinarianId)
                .Select(appointment => appointment.Pet)
                .Distinct()
                .OrderBy(pet => pet.Name)
                .ToArray();
        }
    }

    public Pet Add(string name, int breedId, int ownerId, DateOnly birthDate, double weight)
    {
        lock (store.Sync)
        {
            var breed = FindBreed(breedId);
            var owner = FindOwner(ownerId);
            var pet = new Pet
            {
                Id = store.TakePetId(),
                Name = name,
                Breed = breed,
                Owner = owner,
                BirthDate = birthDate,
                Weight = weight
            };
            store.Pets.Add(pet);
            owner.Pets.Add(pet);
            return pet;
        }
    }

    public Pet Update(int id, string name, int breedId, int ownerId, DateOnly birthDate, double weight)
    {
        lock (store.Sync)
        {
            var pet = store.Pets.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("питомец", id);
            var breed = FindBreed(breedId);
            var owner = FindOwner(ownerId);

            if (store.Appointments.Any(appointment =>
                    appointment.Pet.Id == id
                    && appointment.Veterinarian.Specialization.Species != breed.Species))
            {
                throw new DomainRuleException(
                    "Нельзя сменить породу: вид животного не совпадает со специализацией врача на существующих приёмах.");
            }

            if (!ReferenceEquals(pet.Owner, owner))
            {
                pet.Owner.Pets.Remove(pet);
                owner.Pets.Add(pet);
                pet.Owner = owner;
            }

            pet.Name = name;
            pet.Breed = breed;
            pet.BirthDate = birthDate;
            pet.Weight = weight;
            return pet;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var pet = store.Pets.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("питомец", id);

            if (store.Appointments.Any(appointment => appointment.Pet.Id == id))
            {
                throw new EntityInUseException($"Питомец «{pet.Name}» записан на приём, удаление невозможно.");
            }

            pet.Owner.Pets.Remove(pet);
            store.Pets.Remove(pet);
        }
    }

    private Breed FindBreed(int breedId) =>
        store.Breeds.FirstOrDefault(breed => breed.Id == breedId)
        ?? throw new EntityNotFoundException("порода", breedId);

    private Owner FindOwner(int ownerId) =>
        store.Owners.FirstOrDefault(owner => owner.Id == ownerId)
        ?? throw new EntityNotFoundException("владелец", ownerId);
}
