using System.ComponentModel.DataAnnotations;

namespace VetClinic.Contracts.Validation;

/// <summary>
/// Проверяет, что значение входит в объявленные элементы перечисления
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class DefinedEnumAttribute : ValidationAttribute
{
    public DefinedEnumAttribute()
    {
        ErrorMessage = "Укажите значение из перечисления.";
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        var type = value.GetType();
        return type.IsEnum && Enum.IsDefined(type, value);
    }
}
