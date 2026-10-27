using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Specializations;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class SpecializationService(
    ISpecializationRepository specializations,
    ILogger<SpecializationService> logger) : ISpecializationService
{
    public IReadOnlyList<SpecializationDto> GetAll()
    {
        logger.LogInformation("Получение списка специализаций");
        var result = specializations.GetAll().Select(ClinicMapper.ToSpecialization).ToArray();
        logger.LogInformation("Список специализаций получен, записей: {Count}", result.Length);
        return result;
    }

    public SpecializationDto GetById(int id)
    {
        logger.LogInformation("Получение специализации {SpecializationId}", id);
        var specialization = specializations.FindById(id)
            ?? throw new EntityNotFoundException("специализация", id);
        logger.LogInformation("Специализация {SpecializationId} получена", id);
        return ClinicMapper.ToSpecialization(specialization);
    }

    public IReadOnlyList<SpecializationDto> GetBySpecies(AnimalSpecies species)
    {
        logger.LogInformation("Получение специализаций вида {Species}", species);
        var selected = DtoValidator.RequireSpecies(species);
        var result = specializations.GetBySpecies(selected).Select(ClinicMapper.ToSpecialization).ToArray();
        logger.LogInformation("Специализации вида {Species} получены, записей: {Count}", selected, result.Length);
        return result;
    }

    public SpecializationDto Create(SpecializationEditDto dto)
    {
        logger.LogInformation("Создание специализации");
        DtoValidator.EnsureValid(dto);
        var created = specializations.Add(
            DtoValidator.RequireText(dto.Name, "Укажите название специализации."),
            DtoValidator.RequireSpecies(dto.Species));
        logger.LogInformation("Специализация {SpecializationId} создана", created.Id);
        return ClinicMapper.ToSpecialization(created);
    }

    public SpecializationDto Update(int id, SpecializationEditDto dto)
    {
        logger.LogInformation("Обновление специализации {SpecializationId}", id);
        DtoValidator.EnsureValid(dto);
        var updated = specializations.Update(
            id,
            DtoValidator.RequireText(dto.Name, "Укажите название специализации."),
            DtoValidator.RequireSpecies(dto.Species));
        logger.LogInformation("Специализация {SpecializationId} обновлена", id);
        return ClinicMapper.ToSpecialization(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление специализации {SpecializationId}", id);
        specializations.Remove(id);
        logger.LogInformation("Специализация {SpecializationId} удалена", id);
    }
}
