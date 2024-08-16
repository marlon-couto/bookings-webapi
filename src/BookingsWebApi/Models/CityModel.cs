using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public sealed class CityModel : EntityBase
{
    [Required]
    [MinLength(2)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(25)]
    public string State { get; set; } = string.Empty;

    public IEnumerable<HotelModel>? Hotels { get; set; }
}
