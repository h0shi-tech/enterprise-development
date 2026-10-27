using Microsoft.AspNetCore.Mvc;
using VetClinic.Contracts.Appointments;

namespace VetClinic.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/appointments")]
public class AppointmentsController(IAppointmentService appointments, ILogger<AppointmentsController> logger)
    : ControllerBase
{
    [HttpGet]
    public ActionResult<IReadOnlyList<AppointmentDto>> GetAll()
    {
        logger.LogInformation("GET /api/appointments");
        var result = appointments.GetAll();
        logger.LogInformation("GET /api/appointments завершён, записей: {Count}", result.Count);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<AppointmentDto> GetById(int id)
    {
        logger.LogInformation("GET /api/appointments/{AppointmentId}", id);
        var result = appointments.GetById(id);
        logger.LogInformation("GET /api/appointments/{AppointmentId} завершён", id);
        return Ok(result);
    }

    [HttpPost]
    public ActionResult<AppointmentDto> Create(AppointmentEditDto dto)
    {
        logger.LogInformation("POST /api/appointments");
        var created = appointments.Create(dto);
        logger.LogInformation("POST /api/appointments завершён, идентификатор: {AppointmentId}", created.Id);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public ActionResult<AppointmentDto> Update(int id, AppointmentEditDto dto)
    {
        logger.LogInformation("PUT /api/appointments/{AppointmentId}", id);
        var updated = appointments.Update(id, dto);
        logger.LogInformation("PUT /api/appointments/{AppointmentId} завершён", id);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        logger.LogInformation("DELETE /api/appointments/{AppointmentId}", id);
        appointments.Delete(id);
        logger.LogInformation("DELETE /api/appointments/{AppointmentId} завершён", id);
        return NoContent();
    }
}
