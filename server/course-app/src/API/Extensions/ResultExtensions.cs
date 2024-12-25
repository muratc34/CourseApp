using Domain.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToProblemDetails(this Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        var problemDetails = new ProblemDetails
        {
            Status = GetStatusCode(result.Errors.FirstOrDefault()?.Type ?? ErrorType.Failure),
            Title = GetTitle(result.Errors.FirstOrDefault()?.Type ?? ErrorType.Failure),
            Type = GetType(result.Errors.FirstOrDefault()?.Type ?? ErrorType.Failure),
            Extensions = 
            {
                { "errors", result.Errors.Select(e => new { e.Code, e.Description }) }
            }
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Permission => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

    static string GetTitle(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Failure => "Bad Request",
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Conflict => "Conflict",
            ErrorType.Permission => "Forbidden",
            _ => "Server Failure"
        };

    static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Failure => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Permission => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };
}
