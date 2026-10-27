using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Owners;
using VetClinic.Contracts.Pets;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/owners")]
public class OwnersController(IOwnerService owners, IPetService pets, ILogger<OwnersController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<OwnerDto>> GetAll()
    {
        logger.LogInformation("GET /api/owners");
        var result = owners.GetAll();
        logger.LogInformation("GET /api/owners завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("with-several-pets")]
    public ActionResult<IReadOnlyList<OwnerDto>> GetWithSeveralPets()
    {
        logger.LogInformation("GET /api/owners/with-several-pets");
        var result = owners.GetWithSeveralPets();
        logger.LogInformation("GET /api/owners/with-several-pets завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<OwnerDto> GetById(int id)
    {
        logger.LogInformation("GET /api/owners/{OwnerId}", id);
        var result = owners.GetById(id);
        logger.LogInformation("GET /api/owners/{OwnerId} завершён", id);
        return Ok(result);
    }

    [HttpGet("{id:int}/pets")]
    public ActionResult<IReadOnlyList<PetDto>> GetPets(int id)
    {
        logger.LogInformation("GET /api/owners/{OwnerId}/pets", id);
        var result = pets.GetByOwner(id);
        logger.LogInformation("GET /api/owners/{OwnerId}/pets завершён, записей: {Count}", id, result.Count);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<OwnerDto> Create(OwnerEditDto dto)
    {
        logger.LogInformation("POST /api/owners");
        var created = owners.Create(dto);
        logger.LogInformation("POST /api/owners завершён, идентификатор: {OwnerId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<OwnerDto> Update(int id, OwnerEditDto dto)
    {
        logger.LogInformation("PUT /api/owners/{OwnerId}", id);
        var updated = owners.Update(id, dto);
        logger.LogInformation("PUT /api/owners/{OwnerId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/owners/{OwnerId}", id);
        owners.Delete(id);
        logger.LogInformation("DELETE /api/owners/{OwnerId} завершён", id);
        return NoContent();
    }
}
