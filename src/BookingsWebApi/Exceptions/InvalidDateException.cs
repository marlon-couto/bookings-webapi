namespace BookingsWebApi.Exceptions;

public class InvalidDateException : ArgumentException
{
    public InvalidDateException(string message) : base(message)
    {
    }
}