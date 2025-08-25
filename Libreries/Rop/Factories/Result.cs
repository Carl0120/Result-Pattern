using Rop.Results;

namespace Rop.Factories;

public static class Result
{
    public static ResultAction Success(string successMessage = "OK")
    {
        return new ResultAction(successMessage, ResultCode.Ok);
    }

    public static ResultAction<T> Success<T>(T value, string successMessage = "OK")
    {
        return Result<T>.Success(value);
    }

    public static async Task<ResultAction<T>> Success<T>(Task<T> value, string successMessage = "OK")
    {
        return Result<T>.Success(await value);
    }

    public static ResultAction<T> EnsureFound<T>(T? value, string notFoundMessage)
    {
        return Result<T>.EnsureFound(value, notFoundMessage);
    }

    public static async Task<ResultAction<T>> EnsureFound<T>(Task<T?> value, string notFoundMessage)
    {
        return await Result<T>.EnsureFound(value, notFoundMessage);
    }

    //BadRequest400
    public static ResultAction BadRequest(ErrorValidation error,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction(error, message, ResultCode.BadRequest);
    }

    public static ResultAction BadRequest(IEnumerable<ErrorValidation> error,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction(error.ToList(), message, ResultCode.BadRequest);
    }

    //NotFound404
    public static ResultAction NotFound(string message = "No se encontro en recurso solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.NotFound);
    }

    //Unauthorized401
    public static ResultAction Unauthorized(
        string message = "No esta autorizado para acceder al recurso solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), message,
            ResultCode.Unauthorized);
    }

    //Unauthorized428
    public static ResultAction PASSWORD_CHANGE_REQUIRED(
        string message)
    {
        return new ResultAction(ErrorValidation.Empty(), message,
            ResultCode.PreconditionRequired);
    }

    public static ResultAction Conflict(string message = "Su solicitud no puede ser Procesada")
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.Conflict);
    }
}