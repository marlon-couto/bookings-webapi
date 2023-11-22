using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public class Hotel
{
    [Required] public string HotelId { get; set; } = string.Empty;

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string Address { get; set; } = string.Empty;

    [Required] public string CityId { get; set; } = string.Empty;

    public City? City { get; set; }
    public virtual List<Room>? Rooms { get; set; }
}