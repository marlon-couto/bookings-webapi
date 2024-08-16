namespace BookingsWebApi.DTOs;

public record RoomDto
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public int Capacity { get; init; }
    public string? Image { get; init; }
    public HotelDto? Hotel { get; init; }
}

public record RoomInsertDto
{
    public string? Name { get; init; }
    public int Capacity { get; init; }
    public string? Image { get; init; }
    public Guid? HotelId { get; init; }
}
