using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public class Room : Entity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int Capacity { get; set; }

    [Required]
    public string Image { get; set; } = string.Empty;

    [Required]
    public string HotelId { get; set; } = string.Empty;

    public Hotel? Hotel { get; set; }
    public virtual List<Booking>? Bookings { get; set; }
}
