namespace BookingWebApi.Dtos
{
    public class RoomDto
    {
        public string RoomId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Image { get; set; } = string.Empty;
        public HotelDto? Hotel { get; set; }
    }
}
