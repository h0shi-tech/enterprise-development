using VetClinic.Domain.Data;
using VetClinic.Domain.Enums;

namespace VetClinic.Tests;

/// <summary>
/// Аналитические запросы к данным ветеринарной клиники
/// </summary>
public class VetClinicQueryTests(VetClinicSeedData seed) : IClassFixture<VetClinicSeedData>
{
    /// <summary>
    /// Врачи, специализирующиеся на выбранном биологическом виде животных
    /// </summary>
    [Theory]
    [InlineData(AnimalSpecies.Cat, new[] { 1, 2, 6, 11 })]
    [InlineData(AnimalSpecies.Dog, new[] { 3, 4, 5, 12 })]
    [InlineData(AnimalSpecies.Rodent, new[] { 7 })]
    [InlineData(AnimalSpecies.Rabbit, new[] { 8 })]
    [InlineData(AnimalSpecies.Bird, new[] { 9 })]
    [InlineData(AnimalSpecies.Reptile, new[] { 10 })]
    [InlineData(AnimalSpecies.Ferret, new[] { 13 })]
    [InlineData(AnimalSpecies.Fish, new int[] { })]
    public void VeterinariansOfSelectedSpecies(AnimalSpecies species, int[] expectedIds)
    {
        var actualIds = seed.Veterinarians
            .Where(veterinarian => veterinarian.Specialization.Species == species)
            .Select(veterinarian => veterinarian.Id)
            .ToArray();

        Assert.Equal(expectedIds, actualIds);
    }

    /// <summary>
    /// Питомцы, записанные на приём к указанному врачу, упорядоченные по кличке
    /// </summary>
    [Theory]
    [InlineData(3, new[] { 4, 3, 15 })]
    [InlineData(1, new[] { 1, 16 })]
    [InlineData(8, new[] { 9 })]
    public void PetsOfSelectedVeterinarianOrderedByName(int veterinarianId, int[] expectedIds)
    {
        var actualIds = seed.Appointments
            .Where(appointment => appointment.Veterinarian.Id == veterinarianId)
            .Select(appointment => appointment.Pet)
            .Distinct()
            .OrderBy(pet => pet.Name)
            .Select(pet => pet.Id)
            .ToArray();

        Assert.Equal(expectedIds, actualIds);
    }

    /// <summary>
    /// Количество повторных приёмов животных выбранной породы
    /// </summary>
    [Theory]
    [InlineData(1, 3)]
    [InlineData(5, 2)]
    [InlineData(3, 1)]
    [InlineData(4, 1)]
    [InlineData(9, 1)]
    [InlineData(7, 0)]
    public void FollowUpCountForSelectedBreed(int breedId, int expectedCount)
    {
        var actualCount = seed.Appointments
            .Count(appointment => appointment.IsFollowUp && appointment.Pet.Breed.Id == breedId);

        Assert.Equal(expectedCount, actualCount);
    }

    /// <summary>
    /// Владельцы, имеющие более одного питомца, упорядоченные по ФИО
    /// </summary>
    [Fact]
    public void OwnersWithSeveralPetsOrderedByFullName()
    {
        int[] expectedIds = [2, 5, 7, 1];

        var actualIds = seed.Owners
            .Where(owner => owner.Pets.Count > 1)
            .OrderBy(owner => owner.FullName)
            .Select(owner => owner.Id)
            .ToArray();

        Assert.Equal(expectedIds, actualIds);
    }

    /// <summary>
    /// Приёмы текущего месяца, проходящие в выбранном кабинете
    /// </summary>
    [Theory]
    [InlineData("101", new[] { 13, 15, 23, 24, 28 })]
    [InlineData("102", new[] { 14, 26 })]
    [InlineData("201", new[] { 16, 19, 20 })]
    [InlineData("202", new[] { 17, 21, 25, 27 })]
    [InlineData("301", new int[] { })]
    public void CurrentMonthAppointmentsInSelectedRoom(string roomNumber, int[] expectedIds)
    {
        var monthStart = new DateTime(seed.ReferenceDate.Year, seed.ReferenceDate.Month, 1);
        var nextMonthStart = monthStart.AddMonths(1);

        var actualIds = seed.Appointments
            .Where(appointment => appointment.RoomNumber == roomNumber
                && appointment.VisitDateTime >= monthStart
                && appointment.VisitDateTime < nextMonthStart)
            .OrderBy(appointment => appointment.VisitDateTime)
            .Select(appointment => appointment.Id)
            .ToArray();

        Assert.Equal(expectedIds, actualIds);
    }
}
