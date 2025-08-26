namespace ResultPattern;

/// <summary>
/// Representa un error de validación asociado a un campo, propiedad o identificador lógico.
/// </summary>
public class ErrorValidation
{
    /// <summary>
    /// Identificador del elemento que produjo el error. 
    /// Puede ser un nombre de propiedad, campo, o cualquier clave lógica.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// Mensaje de error asociado al identificador.
    /// Describe por qué ocurrió la falla de validación.
    /// </summary>
    public string ErrorMessage { get; }

    private ErrorValidation(string identifier, string errorMessage)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Crea una instancia vacía de errores de validación.
    /// Generalmente utilizada cuando una operación falla sin errores de validación específicos.
    /// </summary>
    /// <returns>Lista vacía de errores.</returns>
    public static List<ErrorValidation> Empty()
    {
        return [];
    }

    /// <summary>
    /// Crea un nuevo error de validación.
    /// </summary>
    /// <param name="identifier">Identificador del campo o concepto que falló.</param>
    /// <param name="message">Mensaje descriptivo del error.</param>
    /// <returns>Instancia de <see cref="ErrorValidation"/>.</returns>
    public static ErrorValidation Create(string identifier, string message)
    {
        return new ErrorValidation(identifier, message);
    }
}
