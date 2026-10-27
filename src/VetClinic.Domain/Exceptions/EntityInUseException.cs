namespace VetClinic.Domain.Exceptions;

/// <summary>
/// Сущность нельзя удалить или изменить, потому что на неё ссылаются другие записи
/// </summary>
public sealed class EntityInUseException(string message) : Exception(message);
