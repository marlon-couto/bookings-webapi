namespace BookingsWebApi.DTOs;

public record GeolocationDto
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public record GeolocationJsonResponseDto
{
    public string lat { get; set; } = string.Empty;
    public string lon { get; set; } = string.Empty;
}

public record GeolocationHotelDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public int Distance { get; set; }
}
