namespace BookingsWebApi.Exceptions;

public class InvalidEmailException : ArgumentException
{
    public InvalidEmailException(string message) : base(message)
    {
    }
}