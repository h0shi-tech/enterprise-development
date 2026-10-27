using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Pets;
using VetClinic.Contracts.Veterinarians;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/veterinarians")]
public class VeterinariansController(
    IVeterinarianService veterinarians,
    IPetService pets,
    IAppointmentService appointments,
    ILogger<VeterinariansController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<VeterinarianDto>> GetAll()
    {
        logger.LogInformation("GET /api/veterinarians");
        var result = veterinarians.GetAll();
        logger.LogInformation("GET /api/veterinarians завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<VeterinarianDto> GetById(int id)
    {
        logger.LogInformation("GET /api/veterinarians/{VeterinarianId}", id);
        var result = veterinarians.GetById(id);
        logger.LogInformation("GET /api/veterinarians/{VeterinarianId} завершён", id);
        return Ok(result);
    }

    [HttpGet("{id:int}/pets")]
    public ActionResult<IReadOnlyList<PetDto>> GetPets(int id)
    {
        logger.LogInformation("GET /api/veterinarians/{VeterinarianId}/pets", id);
        var result = pets.GetByVeterinarian(id);
        logger.LogInformation(
            "GET /api/veterinarians/{VeterinarianId}/pets завершён, записей: {Count}",
            id,
            result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}/appointments")]
    public ActionResult<IReadOnlyList<AppointmentDto>> GetAppointments(int id)
    {
        logger.LogInformation("GET /api/veterinarians/{VeterinarianId}/appointments", id);
        var result = appointments.GetByVeterinarian(id);
        logger.LogInformation(
            "GET /api/veterinarians/{VeterinarianId}/appointments завершён, записей: {Count}",
            id,
            result.Count);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<VeterinarianDto> Create(VeterinarianEditDto dto)
    {
        logger.LogInformation("POST /api/veterinarians");
        var created = veterinarians.Create(dto);
        logger.LogInformation("POST /api/veterinarians завершён, идентификатор: {VeterinarianId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<VeterinarianDto> Update(int id, VeterinarianEditDto dto)
    {
        logger.LogInformation("PUT /api/veterinarians/{VeterinarianId}", id);
        var updated = veterinarians.Update(id, dto);
        logger.LogInformation("PUT /api/veterinarians/{VeterinarianId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/veterinarians/{VeterinarianId}", id);
        veterinarians.Delete(id);
        logger.LogInformation("DELETE /api/veterinarians/{VeterinarianId} завершён", id);
        return NoContent();
    }
}
