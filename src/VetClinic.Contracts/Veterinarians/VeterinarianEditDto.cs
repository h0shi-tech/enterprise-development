using System.ComponentModel.DataAnnotations;

namespace VetClinic.Contracts.Veterinarians;

public sealed class VeterinarianEditDto
{
    [Required(ErrorMessage = "Укажите номер паспорта.")]
    [StringLength(20, MinimumLength = 5, ErrorMessage = "Номер паспорта должен содержать от 5 до 20 символов.")]
    public required string PassportNumber { get; init; }

    [Required(ErrorMessage = "Укажите ФИО врача.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "ФИО врача должно содержать от 3 до 150 символов.")]
    public required string FullName { get; init; }

    [Required(ErrorMessage = "Укажите специализацию.")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите идентификатор специализации.")]
    public required int? SpecializationId { get; init; }

    [Required(ErrorMessage = "Укажите год рождения.")]
    [Range(1920, 2020, ErrorMessage = "Год рождения должен быть в диапазоне от 1920 до 2020.")]
    public required int? BirthYear { get; init; }

    [Required(ErrorMessage = "Укажите стаж.")]
    [Range(0, 70, ErrorMessage = "Стаж должен быть от 0 до 70 лет.")]
    public required int? ExperienceYears { get; init; }
}
