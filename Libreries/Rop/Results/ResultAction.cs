namespace Rop.Results;

public class ResultAction : ResultActionBase
{
    internal ResultAction(( IEnumerable<ErrorValidation>? errors, string Message, ResultCode StatusCode) data) : base(
        data.errors, data.Message, data.StatusCode)
    {
    }

    internal ResultAction(string message, ResultCode statusCode) : base(message, statusCode)
    {
    }

    internal ResultAction(List<ErrorValidation> error, string message, ResultCode statusCode) : base(error, message,
        statusCode)
    {
    }

    internal ResultAction(ErrorValidation error, string message, ResultCode statusCode) : base(error, message,
        statusCode)
    {
    }

    internal (IEnumerable<ErrorValidation>? validationErrors, string message, ResultCode statusCode) Deconstruct()
    {
        return new ValueTuple<IEnumerable<ErrorValidation>?, string, ResultCode>
        (
            ValidationErrors,
            Message,
            StatusCode
        );
    }
}