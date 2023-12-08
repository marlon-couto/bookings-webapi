using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public sealed class User : Entity
{
    [Required]
    [MinLength(2)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;

    [Required] [MaxLength(25)] public string Salt { get; set; } = string.Empty;

    [Required] [MaxLength(25)] public string Role { get; init; } = string.Empty;

    public IEnumerable<Booking>? Bookings { get; set; }
}