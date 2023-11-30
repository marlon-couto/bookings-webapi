using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public class Booking : Entity
{
    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    [Required]
    public int GuestQuantity { get; set; }

    [Required]
    public string UserId { get; init; } = string.Empty;

    [ForeignKey("UserId")]
    public User? User { get; set; }

    [Required]
    public string RoomId { get; set; } = string.Empty;

    public Room? Room { get; set; }
}
