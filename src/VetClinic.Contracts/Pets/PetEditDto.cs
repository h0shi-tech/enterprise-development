using System.ComponentModel.DataAnnotations;

namespace VetClinic.Contracts.Pets;

public sealed class PetEditDto
{
    [Required(ErrorMessage = "Укажите кличку.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Кличка должна содержать от 1 до 50 символов.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Укажите породу.")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите идентификатор породы.")]
    public required int? BreedId { get; init; }

    [Required(ErrorMessage = "Укажите владельца.")]
    [Range(1, int.MaxValue, ErrorMessage = "Укажите идентификатор владельца.")]
    public required int? OwnerId { get; init; }

    [Required(ErrorMessage = "Укажите дату рождения.")]
    public required DateOnly? BirthDate { get; init; }

    [Required(ErrorMessage = "Укажите вес.")]
    [Range(0.001, 2000, ErrorMessage = "Вес должен быть от 0,001 до 2000 кг.")]
    public required double? Weight { get; init; }
}
