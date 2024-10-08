namespace BookingsWebApi.DTOs;

public record CityDto
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public string? State { get; init; }
}

public record CityInsertDto
{
    public string? Name { get; init; }
    public string? State { get; init; }
}
