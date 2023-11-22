using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public class City
{
    [Required]
    public string CityId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string State { get; set; } = string.Empty;

    public virtual List<Hotel>? Hotels { get; set; }
}
