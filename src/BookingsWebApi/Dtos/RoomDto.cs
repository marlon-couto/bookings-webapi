namespace BookingsWebApi.DTOs;

public record RoomDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Image { get; set; } = string.Empty;
    public HotelDto? Hotel { get; set; }
}

public record RoomInsertDto
{
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; } = 0;
    public string Image { get; set; } = string.Empty;
    public string HotelId { get; set; } = string.Empty;
}
