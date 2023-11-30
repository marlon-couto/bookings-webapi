using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public sealed class Hotel : Entity
{
    [Required]
    [MinLength(2)]
    [MaxLength(25)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MinLength(5)]
    [MaxLength(50)]
    public string Address { get; set; } = string.Empty;

    [Required]
    public string CityId { get; set; } = string.Empty;

    [ForeignKey("CityId")]
    public City? City { get; set; }

    public IEnumerable<Room>? Rooms { get; set; }
}
