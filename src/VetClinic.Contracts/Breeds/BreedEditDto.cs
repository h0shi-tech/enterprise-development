using System.ComponentModel.DataAnnotations;
using VetClinic.Contracts.Validation;
using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Breeds;

public sealed class BreedEditDto
{
    [Required(ErrorMessage = "Укажите название породы.")]
    [StringLength(80, MinimumLength = 1, ErrorMessage = "Название породы должно содержать от 1 до 80 символов.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Укажите биологический вид.")]
    [DefinedEnum]
    public required AnimalSpecies? Species { get; init; }
}
