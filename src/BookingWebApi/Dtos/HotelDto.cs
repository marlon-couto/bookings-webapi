namespace BookingWebApi.Dtos
{
    public class HotelDto
    {
        public string HotelId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityId { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}
