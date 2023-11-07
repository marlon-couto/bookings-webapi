namespace BookingWebApi.Dtos
{
    public class BookingDto
    {
        public string BookingId { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int GuestQuantity { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public string Room { get; set; } = string.Empty;
    }
}
