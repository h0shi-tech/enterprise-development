using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Breeds;

public sealed class BreedDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required AnimalSpecies Species { get; init; }
}
