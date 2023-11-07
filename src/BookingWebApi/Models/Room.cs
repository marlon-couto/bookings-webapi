namespace BookingWebApi.Models
{
    public class Room
    {
        public string RoomId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Image { get; set; } = string.Empty;
        public string HotelId { get; set; } = string.Empty;
        public virtual List<Booking>? Bookings { get; set; }
    }
}
