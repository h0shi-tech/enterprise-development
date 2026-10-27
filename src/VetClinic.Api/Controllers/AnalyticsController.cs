using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;
using VetClinic.Contracts.Veterinarians;
using VetClinic.Domain.Enums;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/analytics")]
public class AnalyticsController(
    IVeterinarianService veterinarians,
    IPetService pets,
    IOwnerService owners,
    IAppointmentService appointments,
    ILogger<AnalyticsController> logger) : ControllerBase
{
    [HttpGet("veterinarians")]
    public ActionResult<IReadOnlyList<VeterinarianDto>> Veterinarians([FromQuery] AnimalSpecies? species)
    {
        logger.LogInformation("GET /api/analytics/veterinarians, вид: {Species}", species);
        if (species is not { } selected || !Enum.IsDefined(selected))
        {
            return BadRequest("Укажите биологический вид.");
        }

        var result = veterinarians.GetBySpecies(selected);
        logger.LogInformation("GET /api/analytics/veterinarians завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("veterinarians/{veterinarianId:int}/pets")]
    public ActionResult<IReadOnlyList<PetDto>> PetsOfVeterinarian(int veterinarianId)
    {
        logger.LogInformation("GET /api/analytics/veterinarians/{VeterinarianId}/pets", veterinarianId);
        var result = pets.GetByVeterinarian(veterinarianId);
        logger.LogInformation(
            "GET /api/analytics/veterinarians/{VeterinarianId}/pets завершён, записей: {Count}",
            veterinarianId,
            result.Count);
        return Ok(result);
    }

    [HttpGet("breeds/{breedId:int}/follow-ups")]
    public ActionResult<FollowUpCountDto> FollowUps(int breedId)
    {
        logger.LogInformation("GET /api/analytics/breeds/{BreedId}/follow-ups", breedId);
        var result = appointments.CountFollowUps(breedId);
        logger.LogInformation("GET /api/analytics/breeds/{BreedId}/follow-ups завершён, количество: {Count}", breedId, result.Count);
        return Ok(result);
    }

    [HttpGet("owners/with-several-pets")]
    public ActionResult<IReadOnlyList<OwnerDto>> OwnersWithSeveralPets()
    {
        logger.LogInformation("GET /api/analytics/owners/with-several-pets");
        var result = owners.GetWithSeveralPets();
        logger.LogInformation("GET /api/analytics/owners/with-several-pets завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("appointments")]
    public ActionResult<IReadOnlyList<AppointmentDto>> CurrentMonthAppointments([FromQuery] string? roomNumber)
    {
        logger.LogInformation("GET /api/analytics/appointments, кабинет: {RoomNumber}", roomNumber);
        if (string.IsNullOrWhiteSpace(roomNumber))
        {
            return BadRequest("Укажите номер кабинета.");
        }

        var result = appointments.GetCurrentMonth(roomNumber);
        logger.LogInformation("GET /api/analytics/appointments завершён, записей: {Count}", result.Count);
        return Ok(result);
    }
}
