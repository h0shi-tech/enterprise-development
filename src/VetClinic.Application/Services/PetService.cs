using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Pets;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class PetService(IPetRepository pets, TimeProvider time, ILogger<PetService> logger) : IPetService
{
    public IReadOnlyList<PetDto> GetAll()
    {
        logger.LogInformation("Получение списка питомцев");
        var result = pets.GetAll().Select(ClinicMapper.ToPet).ToArray();
        logger.LogInformation("Список питомцев получен, записей: {Count}", result.Length);
        return result;
    }

    public PetDto GetById(int id)
    {
        logger.LogInformation("Получение питомца {PetId}", id);
        var pet = pets.FindById(id) ?? throw new EntityNotFoundException("питомец", id);
        logger.LogInformation("Питомец {PetId} получен", id);
        return ClinicMapper.ToPet(pet);
    }

    public IReadOnlyList<PetDto> GetByOwner(int ownerId)
    {
        logger.LogInformation("Получение питомцев владельца {OwnerId}", ownerId);
        var result = pets.GetByOwner(ownerId).Select(ClinicMapper.ToPet).ToArray();
        logger.LogInformation("Питомцы владельца {OwnerId} получены, записей: {Count}", ownerId, result.Length);
        return result;
    }

    public IReadOnlyList<PetDto> GetByVeterinarian(int veterinarianId)
    {
        logger.LogInformation("Получение питомцев врача {VeterinarianId}", veterinarianId);
        var result = pets.GetByVeterinarian(veterinarianId).Select(ClinicMapper.ToPet).ToArray();
        logger.LogInformation(
            "Питомцы врача {VeterinarianId} получены, записей: {Count}",
            veterinarianId,
            result.Length);
        return result;
    }

    public PetDto Create(PetEditDto dto)
    {
        logger.LogInformation("Создание питомца");
        var (name, breedId, ownerId, birthDate, weight) = Read(dto);
        var created = pets.Add(name, breedId, ownerId, birthDate, weight);
        logger.LogInformation("Питомец {PetId} создан", created.Id);
        return ClinicMapper.ToPet(created);
    }

    public PetDto Update(int id, PetEditDto dto)
    {
        logger.LogInformation("Обновление питомца {PetId}", id);
        var (name, breedId, ownerId, birthDate, weight) = Read(dto);
        var updated = pets.Update(id, name, breedId, ownerId, birthDate, weight);
        logger.LogInformation("Питомец {PetId} обновлён", id);
        return ClinicMapper.ToPet(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление питомца {PetId}", id);
        pets.Remove(id);
        logger.LogInformation("Питомец {PetId} удалён", id);
    }

    private (string Name, int BreedId, int OwnerId, DateOnly BirthDate, double Weight) Read(PetEditDto dto)
    {
        DtoValidator.EnsureValid(dto);
        var birthDate = dto.BirthDate ?? throw new DomainRuleException("Укажите дату рождения.");
        var today = DateOnly.FromDateTime(time.GetLocalNow().DateTime);
        if (birthDate > today || birthDate.Year < 1980)
        {
            throw new DomainRuleException("Дата рождения питомца указана некорректно.");
        }

        return (
            DtoValidator.RequireText(dto.Name, "Укажите кличку."),
            DtoValidator.RequireId(dto.BreedId, "Укажите породу."),
            DtoValidator.RequireId(dto.OwnerId, "Укажите владельца."),
            birthDate,
            dto.Weight ?? throw new DomainRuleException("Укажите вес."));
    }
}
