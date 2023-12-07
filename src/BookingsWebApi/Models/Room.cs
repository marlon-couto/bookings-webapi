using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public sealed class Room : Entity
{
    [Required]
    [MinLength(2)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required] [Range(1, 100)] public int Capacity { get; set; }

    [Required] public string Image { get; set; } = string.Empty;

    [Required] public string HotelId { get; set; } = string.Empty;

    [ForeignKey("HotelId")] public Hotel? Hotel { get; set; }

    public IEnumerable<Booking>? Bookings { get; set; }
}