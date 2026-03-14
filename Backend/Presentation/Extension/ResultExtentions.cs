using Microsoft.AspNetCore.Http;
using SharedLib.Models.Common;

namespace Presentation.Extension;

public static class ResultExtentions
{

    public static IResult ToHttpResult(this ApiOperationResult apiOperationResult)
    {
        try
        {
            if (apiOperationResult.IsSuccess)
                return Results.Ok(apiOperationResult);
            else
            {
                var errorType = apiOperationResult.Errors!.First().ErrorType;
                return GetHttpResult(errorType, apiOperationResult);
            }
        }
        catch (Exception ex)
        {
            // log ex here (Serilog, ILogger, etc.)
            return Results.Problem(
                title: "Internal Server Error",
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "An unexpected error occurred."
            );
        }
    }

    public static IResult ToHttpResult<T>(this ApiOperationResult<PagedResult<T>> pagedApiOperationResult)
    {
        try
        {
            IReadOnlyList<ApiOperationError>? apiOperationErrors = pagedApiOperationResult.Errors;
            if (apiOperationErrors is null || !apiOperationErrors.Any())
                return Results.Ok(pagedApiOperationResult);
            else
            {
                var errorType = apiOperationErrors.First().ErrorType;
                return GetHttpResult(errorType, pagedApiOperationResult);
            }
        }
        catch (Exception ex)
        {
            // log ex here (Serilog, ILogger, etc.)
            return Results.Problem(
                title: "Internal Server Error",
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "An unexpected error occurred."
            );
        }
    }


    private static IResult GetHttpResult(ApiErrorType? errorType, object sender)
    {
        if (!errorType.HasValue)
        {
            Exception exception = (Exception)sender;

            Microsoft.AspNetCore.Mvc.ProblemDetails problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Exception",
                Detail = exception.Message,
                Instance = nameof(ToHttpResult),
                Type = exception.GetType().FullName
            };

            return Results.Problem(problemDetails);

        }
        else
        {
            switch (errorType)
            {
                case ApiErrorType.Conflict:
                    return Results.Conflict(sender);
                case ApiErrorType.NotFound:
                    return Results.NotFound(sender);

                default:
                    return Results.BadRequest(sender);
            }
        }
        
    }
}
