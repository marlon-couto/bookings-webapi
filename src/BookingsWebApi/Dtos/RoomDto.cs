namespace BookingsWebApi.Dtos
{
    public class RoomDto
    {
        public string RoomId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Image { get; set; } = string.Empty;
        public HotelDto? Hotel { get; set; }
    }

    public class RoomInsertDto
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Image { get; set; } = string.Empty;
        public string HotelId { get; set; } = string.Empty;
    }
}
