using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingsWebApi.Models;

public sealed class HotelModel : EntityBase
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
    public Guid CityId { get; set; }

    [ForeignKey("CityId")]
    public CityModel? City { get; set; }

    public ICollection<RoomModel>? Rooms { get; set; }
}
