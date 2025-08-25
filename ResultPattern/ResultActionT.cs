using ResultPattern.Exceptions;

namespace ResultPattern;

/// <summary>
/// Representa el resultado de una operación que puede retornar datos de tipo <typeparamref name="T"/>,
/// incluyendo información sobre éxito, errores de validación y códigos de estado.
/// </summary>
/// <typeparam name="T">Tipo de dato esperado en la respuesta.</typeparam>
public class ResultAction<T> : ResultAction
{
    /// <summary>
    /// Datos retornados por la operación, o <c>null</c> si falló o no hay datos.
    /// </summary>
    public T? Data { get; }

    /// <summary>
    /// Devuelve el valor si está presente, o lanza una excepción <see cref="NullResultValueException"/> si es <c>null</c>.
    /// </summary>
    public T EnsureValue => Data ?? throw new NullResultValueException();

    /// <summary>
    /// Indica si la operación fue exitosa y los datos están presentes.
    /// </summary>
    public override bool IsSuccess => base.IsSuccess && Data is not null;

   private ResultAction(T? data, IEnumerable<ErrorValidation>? error, string message, ResultCode statusCode)
        : base(error, message, statusCode)
    {
        Data = data;

        if (Data is null && ValidationErrors is null)
            throw new InvalidResultStatus();
    }

    /// <summary>
    /// Crea un resultado exitoso con datos.
    /// </summary>
    /// <param name="value">Valor retornado.</param>
    /// <param name="message">Mensaje opcional. Por defecto, "Ok".</param>
    public static ResultAction<T> Success(T value, string message = "Ok")
    {
        return new ResultAction<T>(value, null, message, ResultCode.Ok);
    }

    /// <summary>
    /// Crea un resultado de error por solicitud inválida (Bad Request) con un solo error.
    /// </summary>
    /// <param name="error">Error de validación.</param>
    /// <param name="message">Mensaje opcional. Por defecto, "Han ocurrido uno o más errores de validación".</param>
    public new static ResultAction<T> BadRequest(ErrorValidation error,
        string message = "Han ocurrido uno o más errores de validación")
    {
        return new ResultAction<T>(default, [error], message, ResultCode.BadRequest);
    }

    /// <summary>
    /// Crea un resultado de error por solicitud inválida (Bad Request) con múltiples errores.
    /// </summary>
    /// <param name="error">Lista de errores de validación.</param>
    /// <param name="message">Mensaje opcional. Por defecto, "Han ocurrido uno o más errores de validación".</param>
    public new static ResultAction<T> BadRequest(IEnumerable<ErrorValidation> error,
        string message = "Han ocurrido uno o más errores de validación")
    {
        return new ResultAction<T>(default, error.ToList(), message, ResultCode.BadRequest);
    }

    /// <summary>
    /// Crea un resultado de conflicto (Conflict), sin datos.
    /// </summary>
    /// <param name="message">Mensaje opcional. Por defecto, "Su solicitud no puede ser procesada".</param>
    public new static ResultAction<T> Conflict(string message = "Su solicitud no puede ser procesada")
    {
        return new ResultAction<T>(default, ErrorValidation.Empty(), message, ResultCode.Conflict);
    }

    /// <summary>
    /// Devuelve un resultado exitoso si el valor no es <c>null</c>, o un resultado NotFound en caso contrario.
    /// </summary>
    /// <param name="value">Valor esperado.</param>
    /// <param name="notFoundMessage">Mensaje de error si el valor es <c>null</c>.</param>
    public static ResultAction<T> EnsureFound(T? value, string notFoundMessage)
    {
        return value is null ? NotFound(notFoundMessage) : Success(value);
    }

    /// <summary>
    /// Espera una tarea con resultado <typeparamref name="T"/> y devuelve éxito o NotFound según el resultado.
    /// </summary>
    /// <param name="value">Tarea con el valor a evaluar.</param>
    /// <param name="notFoundMessage">Mensaje de error si el resultado es <c>null</c>.</param>
    public static async Task<ResultAction<T>> EnsureFound(Task<T?> value, string notFoundMessage)
    {
        var res = await value;
        return res is null ? NotFound(notFoundMessage) : Success(res);
    }

    /// <summary>
    /// Crea un resultado NotFound (recurso no encontrado), sin datos.
    /// </summary>
    /// <param name="errorMessage">Mensaje opcional. Por defecto, "No se encontró el recurso solicitado".</param>
    public new static ResultAction<T> NotFound(string errorMessage = "No se encontró el recurso solicitado")
    {
        return new ResultAction<T>(default, ErrorValidation.Empty(), errorMessage, ResultCode.NotFound);
    }

    /// <summary>
    /// Crea un resultado Unauthorized (no autorizado), sin datos.
    /// </summary>
    /// <param name="errorMessage">Mensaje opcional. Por defecto, "No está autorizado para acceder al recurso solicitado".</param>
    public new static ResultAction<T> Unauthorized(
        string errorMessage = "No está autorizado para acceder al recurso solicitado")
    {
        return new ResultAction<T>(default, ErrorValidation.Empty(), errorMessage, ResultCode.Unauthorized);
    }

    /// <summary>
    /// Crea un resultado exitoso si el valor no es <c>null</c>; de lo contrario, devuelve NotFound.
    /// </summary>
    /// <param name="value">Valor a evaluar.</param>
    /// <param name="errorMessage">Mensaje si el valor es <c>null</c>.</param>
    public static ResultAction<T> Create(T? value, string errorMessage)
    {
        return value is not null
            ? Success(value)
            : NotFound(errorMessage);
    }
}
