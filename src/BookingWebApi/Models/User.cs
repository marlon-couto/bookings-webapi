using System.ComponentModel.DataAnnotations;

namespace BookingWebApi.Models
{
    public class User
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

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
}
