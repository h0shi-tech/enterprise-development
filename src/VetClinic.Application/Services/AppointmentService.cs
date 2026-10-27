using Microsoft.Extensions.Logging;
using VetClinic.Application.Mapping;
using VetClinic.Application.Validation;
using VetClinic.Contracts.Appointments;
using VetClinic.Domain.Exceptions;
using VetClinic.Domain.Interfaces;

namespace VetClinic.Application.Services;

public sealed class AppointmentService(
    IAppointmentRepository appointments,
    IBreedRepository breeds,
    TimeProvider time,
    ILogger<AppointmentService> logger) : IAppointmentService
{
    public IReadOnlyList<AppointmentDto> GetAll()
    {
        logger.LogInformation("Получение списка записей на приём");
        var result = appointments.GetAll().Select(ClinicMapper.ToAppointment).ToArray();
        logger.LogInformation("Список записей на приём получен, записей: {Count}", result.Length);
        return result;
    }

    public AppointmentDto GetById(int id)
    {
        logger.LogInformation("Получение записи на приём {AppointmentId}", id);
        var appointment = appointments.FindById(id) ?? throw new EntityNotFoundException("запись на приём", id);
        logger.LogInformation("Запись на приём {AppointmentId} получена", id);
        return ClinicMapper.ToAppointment(appointment);
    }

    public IReadOnlyList<AppointmentDto> GetByVeterinarian(int veterinarianId)
    {
        logger.LogInformation("Получение приёмов врача {VeterinarianId}", veterinarianId);
        var result = appointments.GetByVeterinarian(veterinarianId).Select(ClinicMapper.ToAppointment).ToArray();
        logger.LogInformation("Приёмы врача {VeterinarianId} получены, записей: {Count}", veterinarianId, result.Length);
        return result;
    }

    public IReadOnlyList<AppointmentDto> GetByPet(int petId)
    {
        logger.LogInformation("Получение приёмов питомца {PetId}", petId);
        var result = appointments.GetByPet(petId).Select(ClinicMapper.ToAppointment).ToArray();
        logger.LogInformation("Приёмы питомца {PetId} получены, записей: {Count}", petId, result.Length);
        return result;
    }

    public IReadOnlyList<AppointmentDto> GetCurrentMonth(string roomNumber)
    {
        logger.LogInformation("Получение приёмов текущего месяца в кабинете {RoomNumber}", roomNumber);
        var room = DtoValidator.RequireText(roomNumber, "Укажите номер кабинета.");
        var now = time.GetLocalNow().DateTime;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var nextMonth = monthStart.AddMonths(1);
        var result = appointments.GetByRoom(room, monthStart, nextMonth).Select(ClinicMapper.ToAppointment).ToArray();
        logger.LogInformation("Приёмы кабинета {RoomNumber} получены, записей: {Count}", room, result.Length);
        return result;
    }

    public FollowUpCountDto CountFollowUps(int breedId)
    {
        logger.LogInformation("Подсчёт повторных приёмов породы {BreedId}", breedId);
        var breed = breeds.FindById(breedId) ?? throw new EntityNotFoundException("порода", breedId);
        var count = appointments.CountFollowUps(breedId);
        logger.LogInformation("Повторных приёмов породы {BreedId}: {Count}", breedId, count);
        return new FollowUpCountDto
        {
            BreedId = breed.Id,
            BreedName = breed.Name,
            Count = count
        };
    }

    public AppointmentDto Create(AppointmentEditDto dto)
    {
        logger.LogInformation("Создание записи на приём");
        var values = Read(dto);
        var created = appointments.Add(
            values.PetId,
            values.VeterinarianId,
            values.VisitDateTime,
            values.RoomNumber,
            values.IsFollowUp);
        logger.LogInformation("Запись на приём {AppointmentId} создана", created.Id);
        return ClinicMapper.ToAppointment(created);
    }

    public AppointmentDto Update(int id, AppointmentEditDto dto)
    {
        logger.LogInformation("Обновление записи на приём {AppointmentId}", id);
        var values = Read(dto);
        var updated = appointments.Update(
            id,
            values.PetId,
            values.VeterinarianId,
            values.VisitDateTime,
            values.RoomNumber,
            values.IsFollowUp);
        logger.LogInformation("Запись на приём {AppointmentId} обновлена", id);
        return ClinicMapper.ToAppointment(updated);
    }

    public void Delete(int id)
    {
        logger.LogInformation("Удаление записи на приём {AppointmentId}", id);
        appointments.Remove(id);
        logger.LogInformation("Запись на приём {AppointmentId} удалена", id);
    }

    private static (int PetId, int VeterinarianId, DateTime VisitDateTime, string RoomNumber, bool IsFollowUp) Read(
        AppointmentEditDto dto)
    {
        DtoValidator.EnsureValid(dto);
        var visitDateTime = dto.VisitDateTime ?? throw new DomainRuleException("Укажите дату и время приёма.");
        if (visitDateTime.Year < 2000 || visitDateTime.Year > 2100)
        {
            throw new DomainRuleException("Дата приёма указана некорректно.");
        }

        return (
            DtoValidator.RequireId(dto.PetId, "Укажите питомца."),
            DtoValidator.RequireId(dto.VeterinarianId, "Укажите врача."),
            visitDateTime,
            DtoValidator.RequireText(dto.RoomNumber, "Укажите номер кабинета."),
            dto.IsFollowUp);
    }
}
