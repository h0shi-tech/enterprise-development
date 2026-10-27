using System.ComponentModel.DataAnnotations;

namespace VetClinic.Contracts.Appointments;

public sealed class AppointmentEditDto
{
    [Required(ErrorMessage = "Укажите питомца.")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите идентификатор питомца.")]
    public required int? PetId { get; init; }

    [Required(ErrorMessage = "Укажите врача.")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите идентификатор врача.")]
    public required int? VeterinarianId { get; init; }

    [Required(ErrorMessage = "Укажите дату и время приёма.")]
    public required DateTime? VisitDateTime { get; init; }

    [Required(ErrorMessage = "Укажите номер кабинета.")]
    [StringLength(10, MinimumLength = 1, ErrorMessage = "Номер кабинета должен содержать от 1 до 10 символов.")]
    public required string RoomNumber { get; init; }

    public bool IsFollowUp { get; init; }
}
