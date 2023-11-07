namespace BookingWebApi.Models
{
    public class Booking
    {
        public string BookingId { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestQuantity { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public string RoomId { get; set; } = string.Empty;
        public Room? Room { get; set; }
    }
}
