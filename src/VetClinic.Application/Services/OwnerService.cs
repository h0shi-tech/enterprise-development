using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Owners;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class OwnerService(IOwnerRepository owners, ILogger<OwnerService> logger) : IOwnerService
{
    public IReadOnlyList<OwnerDto> GetAll()
    {
        logger.LogInformation("Получение списка владельцев");
        var result = owners.GetAll().Select(ClinicMapper.ToOwner).ToArray();
        logger.LogInformation("Список владельцев получен, записей: {Count}", result.Length);
        return result;
    }

    public OwnerDto GetById(int id)
    {
        logger.LogInformation("Получение владельца {OwnerId}", id);
        var owner = owners.FindById(id) ?? throw new EntityNotFoundException("владелец", id);
        logger.LogInformation("Владелец {OwnerId} получен", id);
        return ClinicMapper.ToOwner(owner);
    }

    public IReadOnlyList<OwnerDto> GetWithSeveralPets()
    {
        logger.LogInformation("Получение владельцев с несколькими питомцами");
        var result = owners.GetWithSeveralPets().Select(ClinicMapper.ToOwner).ToArray();
        logger.LogInformation("Владельцы с несколькими питомцами получены, записей: {Count}", result.Length);
        return result;
    }

    public OwnerDto Create(OwnerEditDto dto)
    {
        logger.LogInformation("Создание владельца");
        var created = owners.Add(ReadName(dto), ReadPhone(dto), DtoValidator.OptionalText(dto.Address));
        logger.LogInformation("Владелец {OwnerId} создан", created.Id);
        return ClinicMapper.ToOwner(created);
    }

    public OwnerDto Update(int id, OwnerEditDto dto)
    {
        logger.LogInformation("Обновление владельца {OwnerId}", id);
        var updated = owners.Update(id, ReadName(dto), ReadPhone(dto), DtoValidator.OptionalText(dto.Address));
        logger.LogInformation("Владелец {OwnerId} обновлён", id);
        return ClinicMapper.ToOwner(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление владельца {OwnerId}", id);
        owners.Remove(id);
        logger.LogInformation("Владелец {OwnerId} удалён", id);
    }

    private static string ReadName(OwnerEditDto dto)
    {
        DtoValidator.EnsureValid(dto);
        return DtoValidator.RequireText(dto.FullName, "Укажите ФИО владельца.");
    }

    private static string ReadPhone(OwnerEditDto dto) =>
        DtoValidator.RequireText(dto.PhoneNumber, "Укажите телефон.");
}
