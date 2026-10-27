using System.ComponentModel.DataAnnotations;
using VetClinic.Contracts.Validation;
using VetClinic.Domain.Enums;

namespace VetClinic.Contracts.Specializations;

public sealed class SpecializationEditDto
{
    [Required(ErrorMessage = "Укажите название специализации.")]
    [StringLength(120, MinimumLength = 1, ErrorMessage = "Название специализации должно содержать от 1 до 120 символов.")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Укажите биологический вид.")]
    [DefinedEnum]
    public required AnimalSpecies? Species { get; init; }
}
