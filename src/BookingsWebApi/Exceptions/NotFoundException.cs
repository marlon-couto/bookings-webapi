namespace BookingsWebApi.Exceptions;

public class NotFoundException : KeyNotFoundException
{
    public NotFoundException(string message)
        : base(message) { }
}
