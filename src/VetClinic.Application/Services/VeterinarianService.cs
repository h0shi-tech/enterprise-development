using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Veterinarians;
using VetClinic.Domain.Enums;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class VeterinarianService(
    IVeterinarianRepository veterinarians,
    TimeProvider time,
    ILogger<VeterinarianService> logger) : IVeterinarianService
{
    public IReadOnlyList<VeterinarianDto> GetAll()
    {
        logger.LogInformation("Получение списка ветеринаров");
        var result = veterinarians.GetAll().Select(ClinicMapper.ToVeterinarian).ToArray();
        logger.LogInformation("Список ветеринаров получен, записей: {Count}", result.Length);
        return result;
    }

    public VeterinarianDto GetById(int id)
    {
        logger.LogInformation("Получение ветеринара {VeterinarianId}", id);
        var veterinarian = veterinarians.FindById(id) ?? throw new EntityNotFoundException("ветеринар", id);
        logger.LogInformation("Ветеринар {VeterinarianId} получен", id);
        return ClinicMapper.ToVeterinarian(veterinarian);
    }

    public IReadOnlyList<VeterinarianDto> GetBySpecies(AnimalSpecies species)
    {
        logger.LogInformation("Получение ветеринаров вида {Species}", species);
        var selected = DtoValidator.RequireSpecies(species);
        var result = veterinarians.GetBySpecies(selected).Select(ClinicMapper.ToVeterinarian).ToArray();
        logger.LogInformation("Ветеринары вида {Species} получены, записей: {Count}", selected, result.Length);
        return result;
    }

    public IReadOnlyList<VeterinarianDto> GetBySpecialization(int specializationId)
    {
        logger.LogInformation("Получение ветеринаров специализации {SpecializationId}", specializationId);
        var result = veterinarians.GetBySpecialization(specializationId).Select(ClinicMapper.ToVeterinarian).ToArray();
        logger.LogInformation(
            "Ветеринары специализации {SpecializationId} получены, записей: {Count}",
            specializationId,
            result.Length);
        return result;
    }

    public VeterinarianDto Create(VeterinarianEditDto dto)
    {
        logger.LogInformation("Создание ветеринара");
        var values = Read(dto);
        var created = veterinarians.Add(
            values.PassportNumber,
            values.FullName,
            values.SpecializationId,
            values.BirthYear,
            values.ExperienceYears);
        logger.LogInformation("Ветеринар {VeterinarianId} создан", created.Id);
        return ClinicMapper.ToVeterinarian(created);
    }

    public VeterinarianDto Update(int id, VeterinarianEditDto dto)
    {
        logger.LogInformation("Обновление ветеринара {VeterinarianId}", id);
        var values = Read(dto);
        var updated = veterinarians.Update(
            id,
            values.PassportNumber,
            values.FullName,
            values.SpecializationId,
            values.BirthYear,
            values.ExperienceYears);
        logger.LogInformation("Ветеринар {VeterinarianId} обновлён", id);
        return ClinicMapper.ToVeterinarian(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление ветеринара {VeterinarianId}", id);
        veterinarians.Remove(id);
        logger.LogInformation("Ветеринар {VeterinarianId} удалён", id);
    }

    private (string PassportNumber, string FullName, int SpecializationId, int BirthYear, int ExperienceYears) Read(
        VeterinarianEditDto dto)
    {
        DtoValidator.EnsureValid(dto);
        var birthYear = dto.BirthYear ?? throw new DomainRuleException("Укажите год рождения.");
        var experienceYears = dto.ExperienceYears ?? throw new DomainRuleException("Укажите стаж.");
        var age = time.GetLocalNow().Year - birthYear;
        if (age < 18)
        {
            throw new DomainRuleException("Год рождения врача указан некорректно.");
        }

        if (experienceYears > age)
        {
            throw new DomainRuleException("Стаж не может превышать возраст.");
        }

        return (
            DtoValidator.RequireText(dto.PassportNumber, "Укажите номер паспорта."),
            DtoValidator.RequireText(dto.FullName, "Укажите ФИО врача."),
            DtoValidator.RequireId(dto.SpecializationId, "Укажите специализацию."),
            birthYear,
            experienceYears);
    }
}
