namespace ResultPattern;

/// <summary>
/// Representa un código de resultado utilizado para describir el estado de una operación.
/// Similar a los códigos HTTP estándar, pero aplicable a cualquier capa de dominio o aplicación.
/// </summary>
public record ResultCode
{
    /// <summary>
    /// Identificador del código, generalmente compatible con códigos HTTP (por ejemplo, 200, 400, 404).
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Nombre o descripción textual del código (por ejemplo, "OK", "Bad Request").
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructor privado para asegurar que solo se puedan crear instancias desde dentro de la clase.
    /// </summary>
    /// <param name="id">Identificador del código.</param>
    /// <param name="name">Nombre descriptivo del código.</param>
    private ResultCode(int id, string name)
    {
        Name = name;
        Id = id;
    }

    /// <summary>
    /// Código que indica que la operación fue exitosa (200).
    /// </summary>
    public static readonly ResultCode Ok = new(200, "OK");

    /// <summary>
    /// Código que indica que la solicitud del cliente es inválida (400).
    /// </summary>
    public static readonly ResultCode BadRequest = new(400, "Bad Request");

    /// <summary>
    /// Código que indica que el cliente no está autorizado (401).
    /// </summary>
    public static readonly ResultCode Unauthorized = new(401, "Unauthorized");

    /// <summary>
    /// Código que indica que el recurso solicitado no fue encontrado (404).
    /// </summary>
    public static readonly ResultCode NotFound = new(404, "Not Found");

    /// <summary>
    /// Código que indica que la solicitud genera un conflicto con el estado actual del recurso (409).
    /// </summary>
    public static readonly ResultCode Conflict = new(409, "Conflict");

    /// <summary>
    /// Código que indica que el acceso está prohibido, aunque el usuario esté autenticado (403).
    /// </summary>
    public static readonly ResultCode Forbidden = new(403, "Forbidden");

    /// <summary>
    /// Código que indica que se requiere cumplir una precondición antes de ejecutar la operación (428).
    /// </summary>
    public static readonly ResultCode PreconditionRequired = new(428, "Precondition Required");

    /// <summary>
    /// Código que indica que ocurrió un error interno en el servidor (500).
    /// </summary>
    public static readonly ResultCode InternalServerError = new(500, "Internal Server Error");

    /// <summary>
    /// Código que indica que la funcionalidad solicitada aún no ha sido implementada (501).
    /// </summary>
    public static readonly ResultCode NotImplemented = new(501, "Not Implemented");
}
