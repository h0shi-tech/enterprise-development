namespace VetClinic.Contracts.Appointments;

public sealed class FollowUpCountDto
{
    public required int BreedId { get; init; }

    public required string BreedName { get; init; }

    public required int Count { get; init; }
}
