namespace VetClinic.Contracts.Appointments;

public sealed class AppointmentDto
{
    public required int Id { get; init; }

    public required int PetId { get; init; }

    public required string PetName { get; init; }

    public required int VeterinarianId { get; init; }

    public required string VeterinarianFullName { get; init; }

    public required DateTime VisitDateTime { get; init; }

    public required string RoomNumber { get; init; }

    public required bool IsFollowUp { get; init; }
}
