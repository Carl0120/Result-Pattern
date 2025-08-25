namespace Rop.Results;

public abstract class ResultActionBase
{
    public string Message { get; private set; }

    public ResultCode StatusCode { get; private set; }

    public IReadOnlyList<ErrorValidation>? ValidationErrors { get; }

    public virtual bool IsSuccess => ValidationErrors == null;
    public bool IsFailure => !IsSuccess;

    protected ResultActionBase(IEnumerable<ErrorValidation>? errors, string message, ResultCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
        ValidationErrors = errors?.ToList();
    }

    protected ResultActionBase(ErrorValidation error, string message, ResultCode statusCode)
        : this(new List<ErrorValidation> { error }, message, statusCode)
    {
    }

    protected ResultActionBase(string message, ResultCode statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}