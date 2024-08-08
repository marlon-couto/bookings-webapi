namespace BookingsWebApi.Exceptions;

public class InvalidInputDataException : ArgumentException
{
    public InvalidInputDataException(string message) : base(message)
    {
    }
}