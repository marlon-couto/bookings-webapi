using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public class BookingModel : EntityBase
{
    [Required] public DateTime CheckIn { get; set; }

    [Required] public DateTime CheckOut { get; set; }

    [Required] public int GuestQuantity { get; set; }

    [Required] [MaxLength(16)] public string UserId { get; init; } = string.Empty;

    [ForeignKey("UserId")] public UserModel? User { get; set; }

    [Required] [MaxLength(16)] public string RoomId { get; set; } = string.Empty;

    public RoomModel? Room { get; set; }
}