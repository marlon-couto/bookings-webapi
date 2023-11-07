namespace BookingWebApi.Models
{
    public class City
    {
        public string CityId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public virtual List<Hotel>? Hotels { get; set; }
    }
}
