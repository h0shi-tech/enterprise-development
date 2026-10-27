using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Specializations;

public sealed class SpecializationDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required AnimalSpecies Species { get; init; }
}
