using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Breeds;
using VetClinic.Contracts.Specializations;
using VetClinic.Contracts.Veterinarians;
using VetClinic.Domain.Enums;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/species/{species}")]
public class SpeciesController(
    IBreedService breeds,
    ISpecializationService specializations,
    IVeterinarianService veterinarians,
    ILogger<SpeciesController> logger) : ControllerBase
{
    [HttpGet("breeds")]
    public ActionResult<IReadOnlyList<BreedDto>> Breeds(AnimalSpecies species)
    {
        logger.LogInformation("GET /api/species/{Species}/breeds", species);
        if (!Enum.IsDefined(species))
        {
            return BadRequest("Укажите биологический вид.");
        }

        var result = breeds.GetBySpecies(species);
        logger.LogInformation("GET /api/species/{Species}/breeds завершён, записей: {Count}", species, result.Count);
        return Ok(result);
    }

    [HttpGet("specializations")]
    public ActionResult<IReadOnlyList<SpecializationDto>> Specializations(AnimalSpecies species)
    {
        logger.LogInformation("GET /api/species/{Species}/specializations", species);
        if (!Enum.IsDefined(species))
        {
            return BadRequest("Укажите биологический вид.");
        }

        var result = specializations.GetBySpecies(species);
        logger.LogInformation(
            "GET /api/species/{Species}/specializations завершён, записей: {Count}",
            species,
            result.Count);
        return Ok(result);
    }

    [HttpGet("veterinarians")]
    public ActionResult<IReadOnlyList<VeterinarianDto>> Veterinarians(AnimalSpecies species)
    {
        logger.LogInformation("GET /api/species/{Species}/veterinarians", species);
        if (!Enum.IsDefined(species))
        {
            return BadRequest("Укажите биологический вид.");
        }

        var result = veterinarians.GetBySpecies(species);
        logger.LogInformation(
            "GET /api/species/{Species}/veterinarians завершён, записей: {Count}",
            species,
            result.Count);
        return Ok(result);
    }
}
