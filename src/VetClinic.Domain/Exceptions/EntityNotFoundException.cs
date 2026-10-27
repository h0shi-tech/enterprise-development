namespace VetClinic.Domain.Exceptions;

/// <summary>
/// Сущность с указанным идентификатором отсутствует в хранилище
/// </summary>
public sealed class EntityNotFoundException(string entityName, int id)
    : Exception($"Не удалось найти {entityName} с идентификатором {id}.")
{
    public string EntityName { get; } = entityName;

    public int EntityId { get; } = id;
}
