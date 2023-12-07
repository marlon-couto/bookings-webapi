namespace BookingsWebApi.DTOs;

public record BookingDto
{
    public string Id { get; set; } = string.Empty; // TODO: replace with GUID.
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestQuantity { get; set; }
    public RoomDto? Room { get; set; }
}

public record BookingInsertDto
{
    public string CheckIn { get; set; } = string.Empty;
    public string CheckOut { get; set; } = string.Empty;
    public int GuestQuantity { get; set; } = 0;
    public string RoomId { get; set; } = string.Empty;
}