namespace ResultPattern.Exceptions;

/// <summary>
/// Se lanza cuando se intenta acceder al valor de un <see cref="ResultAction{T}"/> cuyo resultado es <c>null</c>.
/// Esto indica que la operación no fue exitosa o no devolvió datos.
/// </summary>
public class NullResultValueException : Exception
{
    private const string DefaultMessage = "El valor del ResultAction<T> al que intenta acceder es null. " +
                                          "Use 'EnsureValue' solo cuando el resultado contenga datos válidos.";

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="NullResultValueException"/> con un mensaje por defecto.
    /// </summary>
    public NullResultValueException() : base(DefaultMessage) { }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="NullResultValueException"/> con un mensaje personalizado.
    /// </summary>
    public NullResultValueException(string message) : base(message) { }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="NullResultValueException"/> con un mensaje personalizado y una excepción interna.
    /// </summary>
    public NullResultValueException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Se lanza cuando se intenta crear un <see cref="ResultAction{T}"/> sin un valor de retorno ni errores de validación,
/// lo cual representa un estado inválido.
/// </summary>
public class InvalidResultStatus : Exception
{
    private const string DefaultMessage = "Intento de crear un ResultAction<T> con un estado inválido. " +
                                          "Debe proporcionar al menos un valor válido o errores de validación.";

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="InvalidResultStatus"/> con un mensaje por defecto.
    /// </summary>
    public InvalidResultStatus() : base(DefaultMessage) { }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="InvalidResultStatus"/> con un mensaje personalizado.
    /// </summary>
    public InvalidResultStatus(string message) : base(message) { }

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="InvalidResultStatus"/> con un mensaje personalizado y una excepción interna.
    /// </summary>
    public InvalidResultStatus(string message, Exception innerException) : base(message, innerException) { }
}
