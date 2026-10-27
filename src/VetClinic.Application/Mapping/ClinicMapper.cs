using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Breeds;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;
using VetClinic.Contracts.Specializations;
using VetClinic.Contracts.Veterinarians;
using VetClinic.Domain.Entities;

namespace VetClinic.Application.Mapping;

internal static class ClinicMapper
{
    public static BreedDto ToBreed(Breed breed)
    {
        return new BreedDto
        {
            Id = breed.Id,
            Name = breed.Name,
            Species = breed.Species ?? throw new InvalidOperationException("У породы не указан биологический вид.")
        };
    }

    public static SpecializationDto ToSpecialization(Specialization specialization)
    {
        return new SpecializationDto
        {
            Id = specialization.Id,
            Name = specialization.Name,
            Species = specialization.Species
                ?? throw new InvalidOperationException("У специализации не указан биологический вид.")
        };
    }

    public static OwnerDto ToOwner(Owner owner)
    {
        return new OwnerDto
        {
            Id = owner.Id,
            FullName = owner.FullName,
            PhoneNumber = owner.PhoneNumber,
            Address = owner.Address,
            PetCount = owner.Pets.Count
        };
    }

    public static PetDto ToPet(Pet pet)
    {
        return new PetDto
        {
            Id = pet.Id,
            Name = pet.Name,
            BreedId = pet.Breed.Id,
            BreedName = pet.Breed.Name,
            Species = pet.Species,
            OwnerId = pet.Owner.Id,
            OwnerFullName = pet.Owner.FullName,
            BirthDate = pet.BirthDate,
            Weight = pet.Weight
        };
    }

    public static VeterinarianDto ToVeterinarian(Veterinarian veterinarian)
    {
        return new VeterinarianDto
        {
            Id = veterinarian.Id,
            PassportNumber = veterinarian.PassportNumber,
            FullName = veterinarian.FullName,
            SpecializationId = veterinarian.Specialization.Id,
            SpecializationName = veterinarian.Specialization.Name,
            Species = veterinarian.Specialization.Species
                ?? throw new InvalidOperationException("У специализации не указан биологический вид."),
            BirthYear = veterinarian.BirthYear,
            ExperienceYears = veterinarian.ExperienceYears
        };
    }

    public static AppointmentDto ToAppointment(Appointment appointment)
    {
        return new AppointmentDto
        {
            Id = appointment.Id,
            PetId = appointment.Pet.Id,
            PetName = appointment.Pet.Name,
            VeterinarianId = appointment.Veterinarian.Id,
            VeterinarianFullName = appointment.Veterinarian.FullName,
            VisitDateTime = appointment.VisitDateTime,
            RoomNumber = appointment.RoomNumber,
            IsFollowUp = appointment.IsFollowUp
        };
    }
}
