using System.ComponentModel.DataAnnotations;

namespace VetClinic.Contracts.Owners;

public sealed class OwnerEditDto
{
    [Required(ErrorMessage = "Укажите ФИО владельца.")]
    [StringLength(150, MinimumLength = 3, ErrorMessage = "ФИО владельца должно содержать от 3 до 150 символов.")]
    public required string FullName { get; init; }

    [Required(ErrorMessage = "Укажите телефон.")]
    [Phone(ErrorMessage = "Укажите телефон в корректном формате.")]
    [StringLength(30, MinimumLength = 5, ErrorMessage = "Телефон должен содержать от 5 до 30 символов.")]
    public required string PhoneNumber { get; init; }

    [StringLength(200, ErrorMessage = "Адрес не длиннее 200 символов.")]
    public string? Address { get; init; }
}
