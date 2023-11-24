namespace BookingsWebApi.DTOs;

public record CityDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}

public record CityInsertDto
{
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
