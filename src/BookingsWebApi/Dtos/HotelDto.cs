namespace BookingsWebApi.DTOs;

public record HotelDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityName { get; set; } = string.Empty;
    public string CityState { get; set; } = string.Empty;
}

public record HotelInsertDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string CityId { get; set; } = string.Empty;
}
