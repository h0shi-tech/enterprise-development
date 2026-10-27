using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Breeds;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/breeds")]
public class BreedsController(IBreedService breeds, ILogger<BreedsController> logger) : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<BreedDto>> GetAll()
    {
        logger.LogInformation("GET /api/breeds");
        var result = breeds.GetAll();
        logger.LogInformation("GET /api/breeds завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<BreedDto> GetById(int id)
    {
        logger.LogInformation("GET /api/breeds/{BreedId}", id);
        var result = breeds.GetById(id);
        logger.LogInformation("GET /api/breeds/{BreedId} завершён", id);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<BreedDto> Create(BreedEditDto dto)
    {
        logger.LogInformation("POST /api/breeds");
        var created = breeds.Create(dto);
        logger.LogInformation("POST /api/breeds завершён, идентификатор: {BreedId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<BreedDto> Update(int id, BreedEditDto dto)
    {
        logger.LogInformation("PUT /api/breeds/{BreedId}", id);
        var updated = breeds.Update(id, dto);
        logger.LogInformation("PUT /api/breeds/{BreedId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/breeds/{BreedId}", id);
        breeds.Delete(id);
        logger.LogInformation("DELETE /api/breeds/{BreedId} завершён", id);
        return NoContent();
    }
}
