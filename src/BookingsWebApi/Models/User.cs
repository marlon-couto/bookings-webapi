using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public class User : Entity
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    public virtual List<Booking>? Bookings { get; set; }
}
