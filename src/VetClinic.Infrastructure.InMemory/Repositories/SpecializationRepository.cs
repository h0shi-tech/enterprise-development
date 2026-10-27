using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class SpecializationRepository(InMemoryClinicStore store) : ISpecializationRepository
{
    public IReadOnlyList<Specialization> GetAll()
    {
        lock (store.Sync)
        {
            return store.Specializations.ToArray();
        }
    }

    public Specialization? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Specializations.FirstOrDefault(specialization => specialization.Id == id);
        }
    }

    public IReadOnlyList<Specialization> GetBySpecies(AnimalSpecies species)
    {
        lock (store.Sync)
        {
            return store.Specializations.Where(specialization => specialization.Species == species).ToArray();
        }
    }

    public Specialization Add(string name, AnimalSpecies species)
    {
        lock (store.Sync)
        {
            var specialization = new Specialization
            {
                Id = store.TakeSpecializationId(),
                Name = name,
                Species = species
            };
            store.Specializations.Add(specialization);
            return specialization;
        }
    }

    public Specialization Update(int id, string name, AnimalSpecies species)
    {
        lock (store.Sync)
        {
            var specialization = store.Specializations.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("специализация", id);

            if (specialization.Species != species && HasAppointmentOfAnotherSpecies(id, species))
            {
                throw new DomainRuleException(
                    "Нельзя сменить вид специализации: у врачей этого профиля есть приёмы животных другого вида.");
            }

            specialization.Name = name;
            specialization.Species = species;
            return specialization;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var specialization = store.Specializations.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("специализация", id);

            if (store.Veterinarians.Any(veterinarian => veterinarian.Specialization.Id == id))
            {
                throw new EntityInUseException(
                    $"Специализация «{specialization.Name}» назначена врачам, удаление невозможно.");
            }

            store.Specializations.Remove(specialization);
        }
    }

    private bool HasAppointmentOfAnotherSpecies(int specializationId, AnimalSpecies species)
    {
        return store.Appointments.Any(appointment =>
            appointment.Veterinarian.Specialization.Id == specializationId
            && appointment.Pet.Species != species);
    }
}
