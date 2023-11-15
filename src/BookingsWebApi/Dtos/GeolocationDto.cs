namespace BookingsWebApi.Dtos
{
    public class GeolocationDto
    {
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }

    public class GeolocationJsonResponseDto
    {
        public string lat { get; set; } = string.Empty;
        public string lon { get; set; } = string.Empty;
    }

    public class HotelGeolocationDto
    {
        public string HotelId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public int Distance { get; set; }
    }
}
