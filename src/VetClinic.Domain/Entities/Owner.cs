namespace VetClinic.Domain.Entities;

/// <summary>
/// Владелец питомцев
/// </summary>
public class Owner
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ФИО владельца
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Контактный телефон
    /// </summary>
    public required string PhoneNumber { get; set; }

    /// <summary>
    /// Адрес проживания; владелец может его не сообщать
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Питомцы владельца
    /// </summary>
    public List<Pet> Pets { get; set; } = [];
}
