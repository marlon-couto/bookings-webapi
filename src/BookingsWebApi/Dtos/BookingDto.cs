namespace BookingsWebApi.DTOs;

public record BookingDto
{
    public Guid? Id { get; set; }
    public DateTime? CheckIn { get; init; }
    public DateTime? CheckOut { get; init; }
    public int? GuestQuantity { get; init; }
    public RoomDto? Room { get; init; }
    public Guid? UserId { get; init; }
}

public record BookingInsertDto
{
    public string? CheckIn { get; init; }
    public string? CheckOut { get; init; }
    public int? GuestQuantity { get; init; }
    public Guid? RoomId { get; init; }
}