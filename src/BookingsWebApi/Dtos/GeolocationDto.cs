namespace BookingsWebApi.DTOs;

public record GeolocationDto
{
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
}

public record GeolocationJsonResponseDto
{
    public string? lat { get; init; }
    public string? lon { get; init; }
}

public record GeolocationHotelDto
{
    public string? Id { get; set; }
    public string? Name { get; init; }
    public string? Address { get; set; }
    public string? CityName { get; set; }
    public string? State { get; set; }
    public int? Distance { get; init; }
}