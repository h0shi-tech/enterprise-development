using VetClinic.Domain.Entities;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Infrastructure.InMemory.Repositories;

public sealed class VeterinarianRepository(InMemoryClinicStore store) : IVeterinarianRepository
{
    public IReadOnlyList<Veterinarian> GetAll()
    {
        lock (store.Sync)
        {
            return store.Veterinarians.ToArray();
        }
    }

    public Veterinarian? FindById(int id)
    {
        lock (store.Sync)
        {
            return store.Veterinarians.FirstOrDefault(veterinarian => veterinarian.Id == id);
        }
    }

    public IReadOnlyList<Veterinarian> GetBySpecies(AnimalSpecies species)
    {
        lock (store.Sync)
        {
            return store.Veterinarians
                .Where(veterinarian => veterinarian.Specialization.Species == species)
                .ToArray();
        }
    }

    public IReadOnlyList<Veterinarian> GetBySpecialization(int specializationId)
    {
        lock (store.Sync)
        {
            _ = store.Specializations.FirstOrDefault(specialization => specialization.Id == specializationId)
                ?? throw new EntityNotFoundException("специализация", specializationId);

            return store.Veterinarians
                .Where(veterinarian => veterinarian.Specialization.Id == specializationId)
                .ToArray();
        }
    }

    public Veterinarian Add(
        string passportNumber,
        string fullName,
        int specializationId,
        int birthYear,
        int experienceYears)
    {
        lock (store.Sync)
        {
            var veterinarian = new Veterinarian
            {
                Id = store.TakeVeterinarianId(),
                PassportNumber = passportNumber,
                FullName = fullName,
                Specialization = FindSpecialization(specializationId),
                BirthYear = birthYear,
                ExperienceYears = experienceYears
            };
            store.Veterinarians.Add(veterinarian);
            return veterinarian;
        }
    }

    public Veterinarian Update(
        int id,
        string passportNumber,
        string fullName,
        int specializationId,
        int birthYear,
        int experienceYears)
    {
        lock (store.Sync)
        {
            var veterinarian = store.Veterinarians.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("ветеринар", id);
            var specialization = FindSpecialization(specializationId);

            if (store.Appointments.Any(appointment =>
                    appointment.Veterinarian.Id == id
                    && appointment.Pet.Species != specialization.Species))
            {
                throw new DomainRuleException(
                    "Нельзя сменить специализацию: есть приёмы питомцев другого вида.");
            }

            veterinarian.PassportNumber = passportNumber;
            veterinarian.FullName = fullName;
            veterinarian.Specialization = specialization;
            veterinarian.BirthYear = birthYear;
            veterinarian.ExperienceYears = experienceYears;
            return veterinarian;
        }
    }

    public void Remove(int id)
    {
        lock (store.Sync)
        {
            var veterinarian = store.Veterinarians.FirstOrDefault(item => item.Id == id)
                ?? throw new EntityNotFoundException("ветеринар", id);

            if (store.Appointments.Any(appointment => appointment.Veterinarian.Id == id))
            {
                throw new EntityInUseException(
                    $"У врача «{veterinarian.FullName}» есть записи на приём, удаление невозможно.");
            }

            store.Veterinarians.Remove(veterinarian);
        }
    }

    private Specialization FindSpecialization(int specializationId) =>
        store.Specializations.FirstOrDefault(specialization => specialization.Id == specializationId)
        ?? throw new EntityNotFoundException("специализация", specializationId);
}
