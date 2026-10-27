namespace VetClinic.Contracts.Owners;

public sealed class OwnerDto
{
    public required int Id { get; init; }

    public required string FullName { get; init; }

    public required string PhoneNumber { get; init; }

    public string? Address { get; init; }

    public required int PetCount { get; init; }
}
