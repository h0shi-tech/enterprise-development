using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Breeds;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class BreedService(IBreedRepository breeds, ILogger<BreedService> logger) : IBreedService
{
    public IReadOnlyList<BreedDto> GetAll()
    {
        logger.LogInformation("Получение списка пород");
        var result = breeds.GetAll().Select(ClinicMapper.ToBreed).ToArray();
        logger.LogInformation("Список пород получен, записей: {Count}", result.Length);
        return result;
    }

    public BreedDto GetById(int id)
    {
        logger.LogInformation("Получение породы {BreedId}", id);
        var breed = breeds.FindById(id) ?? throw new EntityNotFoundException("порода", id);
        logger.LogInformation("Порода {BreedId} получена", id);
        return ClinicMapper.ToBreed(breed);
    }

    public IReadOnlyList<BreedDto> GetBySpecies(AnimalSpecies species)
    {
        logger.LogInformation("Получение пород вида {Species}", species);
        var selected = DtoValidator.RequireSpecies(species);
        var result = breeds.GetBySpecies(selected).Select(ClinicMapper.ToBreed).ToArray();
        logger.LogInformation("Породы вида {Species} получены, записей: {Count}", selected, result.Length);
        return result;
    }

    public BreedDto Create(BreedEditDto dto)
    {
        logger.LogInformation("Создание породы");
        DtoValidator.EnsureValid(dto);
        var created = breeds.Add(DtoValidator.RequireText(dto.Name, "Укажите название породы."), DtoValidator.RequireSpecies(dto.Species));
        logger.LogInformation("Порода {BreedId} создана", created.Id);
        return ClinicMapper.ToBreed(created);
    }

    public BreedDto Update(int id, BreedEditDto dto)
    {
        logger.LogInformation("Обновление породы {BreedId}", id);
        DtoValidator.EnsureValid(dto);
        var updated = breeds.Update(
            id,
            DtoValidator.RequireText(dto.Name, "Укажите название породы."),
            DtoValidator.RequireSpecies(dto.Species));
        logger.LogInformation("Порода {BreedId} обновлена", id);
        return ClinicMapper.ToBreed(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление породы {BreedId}", id);
        breeds.Remove(id);
        logger.LogInformation("Порода {BreedId} удалена", id);
    }
}
