using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rop.ResultExtensions;
using Rop.Results;

namespace Rop.AspNetCore.ResultExtensions;

public static class ResultExtensions
{
    public static ActionResult MatchToActionResult<TI>(this ResultAction<TI> resultAction)
    {
        if (!resultAction.IsSuccess) return MatchErrorResult(resultAction.Map());
        return resultAction.StatusCode.Id switch
        {
            StatusCodes.Status200OK => new OkObjectResult(resultAction.EnsureValue),
            _ => MatchErrorResult(resultAction.Map())
        };
    }

    public static ActionResult MatchToActionResult(this ResultAction resultAction)
    {
        if (!resultAction.IsSuccess)
            return MatchErrorResult(resultAction);

        return resultAction.StatusCode.Id switch
        {
            StatusCodes.Status200OK => new OkResult(),
            _ => MatchErrorResult(resultAction)
        };
    }

    private static ActionResult MatchErrorResult(this ResultAction resultAction)
    {
        return resultAction.StatusCode.Id switch
        {
            StatusCodes.Status400BadRequest => new BadRequestObjectResult(resultAction.ToValidationProblemDetails()),
            StatusCodes.Status401Unauthorized => new UnauthorizedObjectResult(
                resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7235#section-3.1")),
            StatusCodes.Status409Conflict => new ConflictObjectResult(
                resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7235#section-3.1")),
            StatusCodes.Status404NotFound => new NotFoundObjectResult(
                resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7231#section-6.5.4")),
            StatusCodes.Status428PreconditionRequired => new ObjectResult(
                resultAction.ToProblemDetails("https://tools.ietf.org/html/rfc7231#section-6.5.4"))
            {
                StatusCode = StatusCodes.Status428PreconditionRequired
            },
            _ => new StatusCodeResult(ResultCode.InternalServerError.Id)
        };
    }

    private static ValidationProblemDetails ToValidationProblemDetails(this ResultActionBase resultAction)
    {
        var validationErrors = resultAction.ValidationErrors ?? new List<ErrorValidation>();
        var errors = validationErrors
            .GroupBy(e => e.Identifier)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
        var problemDetails = new ValidationProblemDetails(errors)
        {
            Status = resultAction.StatusCode.Id,
            Title = resultAction.StatusCode.Name,
            Instance = "",
            Detail = resultAction.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };
        return problemDetails;
    }

    private static ProblemDetails ToProblemDetails(this ResultActionBase resultAction, string type)
    {
        return new ProblemDetails
        {
            Status = resultAction.StatusCode.Id,
            Title = resultAction.StatusCode.Name,
            Instance = "",
            Detail = resultAction.Message,
            Type = type
        };
    }

    public static ActionResult Match<TI>(
        this ResultAction<TI> resultAction,
        Func<TI, ActionResult> onSuccess,
        Func<TI, ActionResult> onError)
    {
        return resultAction.IsSuccess ? onSuccess(resultAction.EnsureValue) : onError(resultAction.EnsureValue);
    }
}