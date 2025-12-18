using System.Net;
using System.Text.Json;

namespace CleanArchitecture.Api.Middleware;
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado: {Message}", ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = 500,
                error = "Internal Server Error",
                message = ex.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }


    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new
        {
            Status = context.Response.StatusCode,
            Message = "Se produjo un error en el servidor.",
            Detail = exception.Message // En producción, no envíes el Detail completo por seguridad
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}