using Rop.Results;

namespace Rop.Factories;

public static class Result<T>
{
    //Success200
    public static ResultAction<T> Success(T value, string message = "Ok")
    {
        if (value is null)
            return new ResultAction<T>(new[] { new ErrorValidation("InvalidData", "Dato Invalido") },
                "No se encontro en recurso solicitado", ResultCode.NotFound);

        return new ResultAction<T>(value, message, ResultCode.Ok);
    }

    //BadRequest400
    public static ResultAction<T> BadRequest(ErrorValidation error,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction<T>(error, message, ResultCode.BadRequest);
    }

    public static ResultAction<T> BadRequest(IEnumerable<ErrorValidation> error,
        string message = "An ocurrido uno o mas errores de Validación")
    {
        return new ResultAction<T>(error.ToList(), message, ResultCode.BadRequest);
    }

    public static ResultAction<T> Conflict(string message = "Su solicitud no puede ser Procesada")
    {
        return new ResultAction<T>(ErrorValidation.Empty(), message, ResultCode.Conflict);
    }

    public static ResultAction<T> EnsureFound(T? value, string notFoundMessage)
    {
        return value is null ? NotFound(notFoundMessage) : Success(value);
    }

    public static async Task<ResultAction<T>> EnsureFound(Task<T?> value, string notFoundMessage)
    {
        var res = await value;
        return res is null ? NotFound(notFoundMessage) : Success(res);
    }

    //NotFound404
    public static ResultAction<T> NotFound(string errorMessage = "No se encontro en recurso solicitado")
    {
        return new ResultAction<T>(ErrorValidation.Empty(), errorMessage, ResultCode.NotFound);
    }

    //Unauthorized401
    public static ResultAction<T> Unauthorized(
        string errorMessage = "No esta autorizado para acceder al recurso solicitado")
    {
        return new ResultAction<T>(ErrorValidation.Empty(), errorMessage, ResultCode.Unauthorized);
    }

    public static ResultAction<T> Create(T? value, string errorMessage)
    {
        if (value is not null)
            return Success(value);

        return NotFound(errorMessage);
    }
}