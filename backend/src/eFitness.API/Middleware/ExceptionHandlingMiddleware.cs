using System.Net;
using eFitness.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ValidationException = eFitness.Application.Common.Exceptions.ValidationException;

namespace eFitness.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (statusCode, problemDetails) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                new ValidationProblemDetails(validationException.Errors)
                {
                    Title = "One or more validation errors occurred.",
                    Status = (int)HttpStatusCode.BadRequest
                } as ProblemDetails),

            NotFoundException notFoundException => (
                HttpStatusCode.NotFound,
                new ProblemDetails { Title = notFoundException.Message, Status = (int)HttpStatusCode.NotFound }),

            ConflictException conflictException => (
                HttpStatusCode.Conflict,
                new ProblemDetails { Title = conflictException.Message, Status = (int)HttpStatusCode.Conflict }),

            ForbiddenAccessException => (
                HttpStatusCode.Forbidden,
                new ProblemDetails { Title = "You do not have permission to perform this action.", Status = (int)HttpStatusCode.Forbidden }),

            UnauthorizedAccessException unauthorizedException => (
                HttpStatusCode.Unauthorized,
                new ProblemDetails { Title = unauthorizedException.Message, Status = (int)HttpStatusCode.Unauthorized }),

            _ => (
                HttpStatusCode.InternalServerError,
                new ProblemDetails { Title = "An unexpected error occurred.", Status = (int)HttpStatusCode.InternalServerError })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception occurred");
        }

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
