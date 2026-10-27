using VetClinic.Domain.Data;
using VetClinic.Domain.Entities;

namespace VetClinic.Infrastructure.InMemory;

/// <summary>
/// Общее хранилище клиники в памяти. Счётчики идентификаторов задаются один раз при старте
/// </summary>
public sealed class InMemoryClinicStore
{
    private int _nextBreedId;
    private int _nextSpecializationId;
    private int _nextOwnerId;
    private int _nextPetId;
    private int _nextVeterinarianId;
    private int _nextAppointmentId;

    public InMemoryClinicStore()
    {
        var seed = new VetClinicSeedData();
        Breeds = seed.Breeds.ToList();
        Specializations = seed.Specializations.ToList();
        Owners = seed.Owners.ToList();
        Pets = seed.Pets.ToList();
        Veterinarians = seed.Veterinarians.ToList();
        Appointments = seed.Appointments.ToList();

        _nextBreedId = NextAfter(Breeds, breed => breed.Id);
        _nextSpecializationId = NextAfter(Specializations, specialization => specialization.Id);
        _nextOwnerId = NextAfter(Owners, owner => owner.Id);
        _nextPetId = NextAfter(Pets, pet => pet.Id);
        _nextVeterinarianId = NextAfter(Veterinarians, veterinarian => veterinarian.Id);
        _nextAppointmentId = NextAfter(Appointments, appointment => appointment.Id);
    }

    public object Sync { get; } = new();

    public List<Breed> Breeds { get; }

    public List<Specialization> Specializations { get; }

    public List<Owner> Owners { get; }

    public List<Pet> Pets { get; }

    public List<Veterinarian> Veterinarians { get; }

    public List<Appointment> Appointments { get; }

    public int TakeBreedId() => _nextBreedId++;

    public int TakeSpecializationId() => _nextSpecializationId++;

    public int TakeOwnerId() => _nextOwnerId++;

    public int TakePetId() => _nextPetId++;

    public int TakeVeterinarianId() => _nextVeterinarianId++;

    public int TakeAppointmentId() => _nextAppointmentId++;

    private static int NextAfter<T>(IReadOnlyList<T> items, Func<T, int> id) =>
        items.Count == 0 ? 1 : items.Max(id) + 1;
}
