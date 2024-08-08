namespace BookingsWebApi.DTOs;

public record HotelDto
{
    public string? Id { get; init; }
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? CityName { get; init; }
    public string? CityState { get; init; }
}

public record HotelInsertDto
{
    public string? Name { get; init; }
    public string? Address { get; init; }
    public string? CityId { get; init; }
}