namespace BookingWebApi.Dtos
{
    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public virtual List<string>? Bookings { get; set; }
    }
}
