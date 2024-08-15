using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public sealed class RoomModel : EntityBase
{
    [Required]
    [MinLength(2)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required] [Range(1, 100)] public int Capacity { get; set; }

    [Required] [MaxLength(100)] public string Image { get; set; } = string.Empty;

    [Required] public Guid HotelId { get; set; }

    [ForeignKey("HotelId")] public HotelModel? Hotel { get; set; }

    public IEnumerable<BookingModel>? Bookings { get; set; }
}