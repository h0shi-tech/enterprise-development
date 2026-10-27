using System.ComponentModel.DataAnnotations;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;

namespace VetClinic.Application.Validation;

internal static class DtoValidator
{
    public static void EnsureValid(object dto)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        if (Validator.TryValidateObject(dto, context, results, validateAllProperties: true))
        {
            return;
        }

        var message = string.Join(" ", results.Select(result => result.ErrorMessage).Where(message => message is not null));
        throw new DomainRuleException(string.IsNullOrWhiteSpace(message) ? "Данные не прошли проверку." : message);
    }

    public static string RequireText(string? value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException(message);
        }

        return value.Trim();
    }

    public static string? OptionalText(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return value.Trim();
    }

    public static AnimalSpecies RequireSpecies(AnimalSpecies? species)
    {
        if (species is not { } value || !Enum.IsDefined(value))
        {
            throw new DomainRuleException("Укажите биологический вид из справочника.");
        }

        return value;
    }

    public static int RequireId(int? id, string message)
    {
        if (id is not > 0)
        {
            throw new DomainRuleException(message);
        }

        return id.Value;
    }
}
