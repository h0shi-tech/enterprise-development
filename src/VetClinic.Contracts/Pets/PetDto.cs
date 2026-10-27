using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Pets;

public sealed class PetDto
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int BreedId { get; init; }

    public required string BreedName { get; init; }

    public required AnimalSpecies Species { get; init; }

    public required int OwnerId { get; init; }

    public required string OwnerFullName { get; init; }

    public required DateOnly BirthDate { get; init; }

    public required double Weight { get; init; }
}
