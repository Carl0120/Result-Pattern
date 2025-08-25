namespace ResultPattern;

/// <summary>
/// Representa el resultado de una acción, incluyendo mensajes, códigos de estado y errores de validación.
/// </summary>
public class ResultAction
{
    /// <summary>
    /// Mensaje descriptivo del resultado.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Código de estado que representa el resultado de la operación.
    /// </summary>
    public ResultCode StatusCode { get; private set; }

    /// <summary>
    /// Lista de errores de validación, si existen.
    /// </summary>
    public IReadOnlyList<ErrorValidation>? ValidationErrors { get; }

    /// <summary>
    /// Indica si la operación fue exitosa (sin errores de validación).
    /// </summary>
    public virtual bool IsSuccess => ValidationErrors == null;

    /// <summary>
    /// Indica si la operación falló (con errores de validación).
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Constructor protegido para controlar la creación de instancias.
    /// </summary>
    protected ResultAction(IEnumerable<ErrorValidation>? errors, string message, ResultCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        ValidationErrors = errors?.ToList();
    }

    /// <summary>
    /// Crea una respuesta exitosa.
    /// </summary>
    /// <param name="successMessage">Mensaje de éxito opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando éxito.</returns>
    public static ResultAction Success(string successMessage = "OK")
    {
        return new ResultAction(null, successMessage, ResultCode.Ok);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static ResultAction<T> Success<T>
        (T value, string message = "Ok")
    {
        return  ResultAction<T>.Success(value, message);
    }

    /// <summary>
    /// Crea una respuesta de error de validación con un solo error.
    /// </summary>
    /// <param name="error">Error de validación.</param>
    /// <param name="message">Mensaje personalizado opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando un Bad Request.</returns>
    public static ResultAction BadRequest(ErrorValidation error,
        string message = "Han ocurrido uno o más errores de validación")
    {
        return new ResultAction([error], message, ResultCode.BadRequest);
    }

    /// <summary>
    /// Crea una respuesta de error de validación con múltiples errores.
    /// </summary>
    /// <param name="error">Colección de errores de validación.</param>
    /// <param name="message">Mensaje personalizado opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando un Bad Request.</returns>
    public static ResultAction BadRequest(IEnumerable<ErrorValidation> error,
        string message = "Han ocurrido uno o más errores de validación")
    {
        return new ResultAction(error.ToList(), message, ResultCode.BadRequest);
    }

    /// <summary>
    /// Crea una respuesta para recurso no encontrado.
    /// </summary>
    /// <param name="message">Mensaje personalizado opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando Not Found.</returns>
    public static ResultAction NotFound(string message = "No se encontró el recurso solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.NotFound);
    }

    /// <summary>
    /// Crea una respuesta de acceso no autorizado.
    /// </summary>
    /// <param name="message">Mensaje personalizado opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando Unauthorized.</returns>
    public static ResultAction Unauthorized(
        string message = "No está autorizado para acceder al recurso solicitado")
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.Unauthorized);
    }

    /// <summary>
    /// Crea una respuesta que indica que el usuario debe cambiar su contraseña.
    /// </summary>
    /// <param name="message">Mensaje personalizado obligatorio.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> con código PreconditionRequired.</returns>
    public static ResultAction PasswordChangeRequired(string message)
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.PreconditionRequired);
    }

    /// <summary>
    /// Crea una respuesta que indica un conflicto (por ejemplo, estado inválido del recurso).
    /// </summary>
    /// <param name="message">Mensaje personalizado opcional.</param>
    /// <returns>Instancia de <see cref="ResultAction"/> representando Conflict.</returns>
    public static ResultAction Conflict(string message = "Su solicitud no puede ser procesada")
    {
        return new ResultAction(ErrorValidation.Empty(), message, ResultCode.Conflict);
    }
    
}
