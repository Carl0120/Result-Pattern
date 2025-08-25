namespace Rop.Results;

public record ResultCode
{
    public int Id { get; }
    public string Name { get; }

    private ResultCode(int id, string name)
    {
        Name = name;
        Id = id;
    }


    public static readonly ResultCode Ok = new(200, "OK");
    public static readonly ResultCode BadRequest = new(400, "Bad Request");
    public static readonly ResultCode Unauthorized = new(401, "Unauthorized");
    public static readonly ResultCode NotFound = new(404, "Not Found");
    public static readonly ResultCode Conflict = new(409, "Conflict");
    public static readonly ResultCode Forbidden = new(403, "Forbidden");
    public static readonly ResultCode PreconditionRequired = new(428, "Precondition Required");
    public static readonly ResultCode InternalServerError = new(500, "Internal Server Error");
    public static readonly ResultCode NotImplemented = new(501, "Not Implemented");
}