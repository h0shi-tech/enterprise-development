using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Specializations;
using VetClinic.Contracts.Veterinarians;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/specializations")]
public class SpecializationsController(
    ISpecializationService specializations,
    IVeterinarianService veterinarians,
    ILogger<SpecializationsController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<SpecializationDto>> GetAll()
    {
        logger.LogInformation("GET /api/specializations");
        var result = specializations.GetAll();
        logger.LogInformation("GET /api/specializations завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<SpecializationDto> GetById(int id)
    {
        logger.LogInformation("GET /api/specializations/{SpecializationId}", id);
        var result = specializations.GetById(id);
        logger.LogInformation("GET /api/specializations/{SpecializationId} завершён", id);
        return Ok(result);
    }

    [HttpGet("{id:int}/veterinarians")]
    public ActionResult<IReadOnlyList<VeterinarianDto>> GetVeterinarians(int id)
    {
        logger.LogInformation("GET /api/specializations/{SpecializationId}/veterinarians", id);
        var result = veterinarians.GetBySpecialization(id);
        logger.LogInformation(
            "GET /api/specializations/{SpecializationId}/veterinarians завершён, записей: {Count}",
            id,
            result.Count);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<SpecializationDto> Create(SpecializationEditDto dto)
    {
        logger.LogInformation("POST /api/specializations");
        var created = specializations.Create(dto);
        logger.LogInformation("POST /api/specializations завершён, идентификатор: {SpecializationId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<SpecializationDto> Update(int id, SpecializationEditDto dto)
    {
        logger.LogInformation("PUT /api/specializations/{SpecializationId}", id);
        var updated = specializations.Update(id, dto);
        logger.LogInformation("PUT /api/specializations/{SpecializationId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/specializations/{SpecializationId}", id);
        specializations.Delete(id);
        logger.LogInformation("DELETE /api/specializations/{SpecializationId} завершён", id);
        return NoContent();
    }
}
