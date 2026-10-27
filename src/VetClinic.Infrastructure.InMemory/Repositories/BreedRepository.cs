using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class BreedRepository(InMemoryClinicStore store) : IBreedRepository
{
    public IReadOnlyList<Breed> GetAll()
    {
        lock (store.Sync)
        {
            return store.Breeds.ToArray();
        }
    }

    public Breed? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Breeds.FirstOrDefault(breed => breed.Id == id);
        }
    }

    public IReadOnlyList<Breed> GetBySpecies(AnimalSpecies species)
    {
        lock (store.Sync)
        {
            return store.Breeds.Where(breed => breed.Species == species).ToArray();
        }
    }

    public Breed Add(string name, AnimalSpecies species)
    {
        lock (store.Sync)
        {
            var breed = new Breed
            {
                Id = store.TakeBreedId(),
                Name = name,
                Species = species
            };
            store.Breeds.Add(breed);
            return breed;
        }
    }

    public Breed Update(int id, string name, AnimalSpecies species)
    {
        lock (store.Sync)
        {
            var breed = store.Breeds.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("порода", id);

            if (breed.Species != species
                && store.Appointments.Any(appointment =>
                    appointment.Pet.Breed.Id == id
                    && appointment.Veterinarian.Specialization.Species != species))
            {
                throw new DomainRuleException(
                    "Нельзя сменить вид породы: у питомцев этой породы есть приёмы у врача другой специализации.");
            }

            breed.Name = name;
            breed.Species = species;
            return breed;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var breed = store.Breeds.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("порода", id);

            if (store.Pets.Any(pet => pet.Breed.Id == id))
            {
                throw new EntityInUseException($"Порода «{breed.Name}» указана у питомцев, удаление невозможно.");
            }

            store.Breeds.Remove(breed);
        }
    }
}
