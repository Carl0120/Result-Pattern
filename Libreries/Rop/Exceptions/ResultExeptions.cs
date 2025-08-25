namespace Rop.Exceptions;

public class NullResultValueException : Exception
{
    public NullResultValueException() : base(
        "El valor del ResultAction al que intenta acceder es nullo")
    {
    }
}

public class InvalidResultStatus : Exception
{
    public InvalidResultStatus() : base(
        "Intento de crear un ResultAction con un estado incorrecto, " +
        "Para crear un ResutAction debe proporcionar un Error o el valor de retorno no puede ser nullo")
    {
    }
}