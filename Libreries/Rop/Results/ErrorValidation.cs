namespace Rop.Results;

public class ErrorValidation
{
    public string Identifier { get; }

    public string ErrorMessage { get; }

    public ErrorValidation(string identifier, string errorMessage)
    {
        Identifier = identifier;
        ErrorMessage = errorMessage;
    }

    public static List<ErrorValidation> Empty()
    {
        return new List<ErrorValidation>();
    }
}