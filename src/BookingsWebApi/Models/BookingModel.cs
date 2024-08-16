using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public class BookingModel : EntityBase
{
    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Required]
    public int GuestQuantity { get; set; }

    [Required]
    public Guid UserId { get; init; }

    [ForeignKey("UserId")]
    public UserModel? User { get; set; }

    [Required]
    public Guid RoomId { get; set; }

    public RoomModel? Room { get; set; }
}
