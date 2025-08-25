using Rop.Exceptions;

namespace Rop.Results;

public class ResultAction<T> : ResultActionBase
{
    public T? Value { get; }

    public T EnsureValue => Value ?? throw new NullResultValueException();

    public override bool IsSuccess => base.IsSuccess && Value is not null;

    internal ResultAction(T? value, ( IEnumerable<ErrorValidation>? errors, string Message, ResultCode StatusCode) data)
        : base(data.errors, data.Message, data.StatusCode)
    {
        Value = value;
        ValidateStatus();
    }

    internal ResultAction(T value, string message, ResultCode statusCode) : base(message, statusCode)
    {
        Value = value;
        ValidateStatus();
    }

    internal ResultAction(ErrorValidation error, string message, ResultCode statusCode) : base(error, message,
        statusCode)
    {
        Value = default;
        ValidateStatus();
    }

    internal ResultAction(IEnumerable<ErrorValidation> error, string message, ResultCode statusCode) : base(error,
        message, statusCode)
    {
        Value = default;
        ValidateStatus();
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

    private void ValidateStatus()
    {
        if (Value is null && ValidationErrors is null)
            throw new InvalidResultStatus();
    }
}