using VetClinic.Domain.Data;

namespace VetClinic.Tests;

/// <summary>
/// Проверки наполнения и связности набора тестовых данных
/// </summary>
public class VetClinicSeedDataTests(VetClinicSeedData seed) : IClassFixture<VetClinicSeedData>
{
    /// <summary>
    /// Каждая сущность представлена не менее чем десятью экземплярами
    /// </summary>
    [Fact]
    public void EachEntityHasAtLeastTenInstances()
    {
        const int minimumCount = 10;

        int[] actualCounts =
        [
            seed.Breeds.Count,
            seed.Specializations.Count,
            seed.Owners.Count,
            seed.Pets.Count,
            seed.Veterinarians.Count,
            seed.Appointments.Count
        ];

        Assert.All(actualCounts, count => Assert.InRange(count, minimumCount, int.MaxValue));
    }

    /// <summary>
    /// Идентификаторы сущностей не повторяются
    /// </summary>
    [Fact]
    public void EntityIdentifiersAreUnique()
    {
        Assert.Equal(seed.Breeds.Count, seed.Breeds.Select(breed => breed.Id).Distinct().Count());
        Assert.Equal(seed.Specializations.Count, seed.Specializations.Select(specialization => specialization.Id).Distinct().Count());
        Assert.Equal(seed.Owners.Count, seed.Owners.Select(owner => owner.Id).Distinct().Count());
        Assert.Equal(seed.Pets.Count, seed.Pets.Select(pet => pet.Id).Distinct().Count());
        Assert.Equal(seed.Veterinarians.Count, seed.Veterinarians.Select(veterinarian => veterinarian.Id).Distinct().Count());
        Assert.Equal(seed.Appointments.Count, seed.Appointments.Select(appointment => appointment.Id).Distinct().Count());
    }

    /// <summary>
    /// Питомец числится в коллекции своего владельца
    /// </summary>
    [Fact]
    public void EveryPetIsListedByItsOwner()
    {
        Assert.All(seed.Pets, pet => Assert.Contains(pet, pet.Owner.Pets));
    }

    /// <summary>
    /// Врач принимает животных того вида, на котором специализируется
    /// </summary>
    [Fact]
    public void EveryAppointmentMatchesVeterinarianSpecies()
    {
        Assert.All(
            seed.Appointments,
            appointment => Assert.Equal(appointment.Veterinarian.Specialization.Species, appointment.Pet.Species));
    }
}
