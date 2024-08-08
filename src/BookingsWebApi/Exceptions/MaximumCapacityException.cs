namespace BookingsWebApi.Exceptions;

public class MaximumCapacityException : ArgumentException
{
    public MaximumCapacityException() : base("The number of guests exceeds the maximum capacity.")
    {
    }
}