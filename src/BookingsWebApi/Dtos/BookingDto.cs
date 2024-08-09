namespace BookingsWebApi.DTOs;

public record BookingDto
{
    public string? Id { get; set; }
    public DateTime CheckIn { get; init; }
    public DateTime CheckOut { get; init; }
    public int GuestQuantity { get; init; }
    public RoomDto? Room { get; init; }
    public string? UserId { get; init; }
}

public record BookingInsertDto
{
    public string? CheckIn { get; init; }
    public string? CheckOut { get; init; }
    public int GuestQuantity { get; init; }
    public string? RoomId { get; init; }
}