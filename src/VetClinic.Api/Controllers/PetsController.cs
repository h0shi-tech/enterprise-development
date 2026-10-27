using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Appointments;
using VetClinic.Contracts.Pets;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/pets")]
public class PetsController(IPetService pets, IAppointmentService appointments, ILogger<PetsController> logger)
    : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<PetDto>> GetAll()
    {
        logger.LogInformation("GET /api/pets");
        var result = pets.GetAll();
        logger.LogInformation("GET /api/pets завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<PetDto> GetById(int id)
    {
        logger.LogInformation("GET /api/pets/{PetId}", id);
        var result = pets.GetById(id);
        logger.LogInformation("GET /api/pets/{PetId} завершён", id);
        return Ok(result);
    }

    [HttpGet("{id:int}/appointments")]
    public ActionResult<IReadOnlyList<AppointmentDto>> GetAppointments(int id)
    {
        logger.LogInformation("GET /api/pets/{PetId}/appointments", id);
        var result = appointments.GetByPet(id);
        logger.LogInformation("GET /api/pets/{PetId}/appointments завершён, записей: {Count}", id, result.Count);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<PetDto> Create(PetEditDto dto)
    {
        logger.LogInformation("POST /api/pets");
        var created = pets.Create(dto);
        logger.LogInformation("POST /api/pets завершён, идентификатор: {PetId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<PetDto> Update(int id, PetEditDto dto)
    {
        logger.LogInformation("PUT /api/pets/{PetId}", id);
        var updated = pets.Update(id, dto);
        logger.LogInformation("PUT /api/pets/{PetId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/pets/{PetId}", id);
        pets.Delete(id);
        logger.LogInformation("DELETE /api/pets/{PetId} завершён", id);
        return NoContent();
    }
}
