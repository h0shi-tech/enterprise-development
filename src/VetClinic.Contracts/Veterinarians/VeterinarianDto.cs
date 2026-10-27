using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Veterinarians;

public sealed class VeterinarianDto
{
    public required int Id { get; init; }

    public required string PassportNumber { get; init; }

    public required string FullName { get; init; }

    public required int SpecializationId { get; init; }

    public required string SpecializationName { get; init; }

    public required AnimalSpecies Species { get; init; }

    public required int BirthYear { get; init; }

    public required int ExperienceYears { get; init; }
}
