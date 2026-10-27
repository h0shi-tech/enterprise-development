using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Breeds;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;

namespace VetClinic.Tests;

public class ClinicServiceTests
{
    [Fact]
    public void VeterinariansOfCat_MatchLabQuery()
    {
        var clinic = new ClinicHarness();

        var ids = clinic.Veterinarians.GetBySpecies(AnimalSpecies.Cat).Select(veterinarian => veterinarian.Id).ToArray();

        Assert.Equal([1, 2, 6, 11], ids);
    }

    [Fact]
    public void PetsOfVeterinarian_AreOrderedByName()
    {
        var clinic = new ClinicHarness();

        var ids = clinic.Pets.GetByVeterinarian(3).Select(pet => pet.Id).ToArray();

        Assert.Equal([4, 3, 15], ids);
    }

    [Fact]
    public void FollowUpsOfBreed_MatchLabQuery()
    {
        var clinic = new ClinicHarness();

        Assert.Equal(3, clinic.Appointments.CountFollowUps(1).Count);
        Assert.Equal(0, clinic.Appointments.CountFollowUps(7).Count);
    }

    [Fact]
    public void OwnersWithSeveralPets_AreOrderedByFullName()
    {
        var clinic = new ClinicHarness();

        var ids = clinic.Owners.GetWithSeveralPets().Select(owner => owner.Id).ToArray();

        Assert.Equal([2, 5, 7, 1], ids);
    }

    [Fact]
    public void SeptemberAppointmentsInRoom101_MatchLabQuery()
    {
        var clock = new FixedTimeProvider(new DateTimeOffset(2026, 9, 15, 12, 0, 0, TimeSpan.Zero));
        var clinic = new ClinicHarness(clock);

        var ids = clinic.Appointments.GetCurrentMonth("101").Select(appointment => appointment.Id).ToArray();

        Assert.Equal([13, 15, 23, 24, 28], ids);
    }

    [Fact]
    public void NewBreed_DoesNotReuseDeletedIdentifier()
    {
        var clinic = new ClinicHarness();
        var created = clinic.Breeds.Create(new BreedEditDto { Name = "Дворняга", Species = AnimalSpecies.Dog });

        clinic.Breeds.Delete(created.Id);
        var next = clinic.Breeds.Create(new BreedEditDto { Name = "Мопс", Species = AnimalSpecies.Dog });

        Assert.NotEqual(created.Id, next.Id);
    }

    [Fact]
    public void DeleteUsedBreed_Throws()
    {
        var clinic = new ClinicHarness();

        var exception = Assert.Throws<EntityInUseException>(() => clinic.Breeds.Delete(1));

        Assert.Contains("Мейн-кун", exception.Message);
    }

    [Fact]
    public void CreateAppointment_RejectsSpeciesMismatch()
    {
        var clinic = new ClinicHarness();

        var exception = Assert.Throws<DomainRuleException>(() => clinic.Appointments.Create(new AppointmentEditDto
        {
            PetId = 1,
            VeterinarianId = 3,
            VisitDateTime = new DateTime(2026, 10, 20, 10, 0, 0),
            RoomNumber = "101",
            IsFollowUp = false
        }));

        Assert.Contains("специализацией", exception.Message);
    }

    [Fact]
    public void CreateBreed_WithoutSpecies_Throws()
    {
        var clinic = new ClinicHarness();

        Assert.Throws<DomainRuleException>(() => clinic.Breeds.Create(new BreedEditDto
        {
            Name = "Беспородная",
            Species = null
        }));
    }

    [Fact]
    public void CreateOwner_RejectsInvalidPhone()
    {
        var clinic = new ClinicHarness();

        Assert.Throws<DomainRuleException>(() => clinic.Owners.Create(new OwnerEditDto
        {
            FullName = "Тестов Тест Тестович",
            PhoneNumber = "не телефон",
            Address = null
        }));
    }

    [Fact]
    public void SeedOwnerPhone_PassesValidation()
    {
        var clinic = new ClinicHarness();
        var owner = clinic.Owners.GetById(1);

        var updated = clinic.Owners.Update(owner.Id, new OwnerEditDto
        {
            FullName = owner.FullName,
            PhoneNumber = owner.PhoneNumber,
            Address = owner.Address
        });

        Assert.Equal(owner.PhoneNumber, updated.PhoneNumber);
    }

    [Fact]
    public void CreatePet_AppearsInOwnerCollection()
    {
        var clinic = new ClinicHarness();

        var created = clinic.Pets.Create(new PetEditDto
        {
            Name = "Бобик",
            BreedId = 4,
            OwnerId = 8,
            BirthDate = new DateOnly(2024, 1, 10),
            Weight = 12.5
        });

        var pets = clinic.Pets.GetByOwner(8);
        Assert.Contains(pets, pet => pet.Id == created.Id);
    }
}
