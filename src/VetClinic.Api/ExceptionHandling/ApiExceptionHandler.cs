using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VetClinic.Domain.Exceptions;

namespace VetClinic.Api.ExceptionHandling;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int? status = exception switch
        {
            EntityNotFoundException => StatusCodes.Status404NotFound,
            EntityInUseException => StatusCodes.Status409Conflict,
            DomainRuleException => StatusCodes.Status400BadRequest,
            _ => null
        };

        if (status is null)
        {
            return false;
        }

        var title = status switch
        {
            StatusCodes.Status404NotFound => "Не найдено",
            StatusCodes.Status409Conflict => "Сущность используется",
            _ => "Нарушено правило предметной области"
        };

        logger.LogWarning(
            exception,
            "Запрос {Method} {Path} завершился ошибкой {StatusCode}",
            httpContext.Request.Method,
            httpContext.Request.Path,
            status);

        httpContext.Response.StatusCode = status.Value;
        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            },
            cancellationToken);

        return true;
    }
}
