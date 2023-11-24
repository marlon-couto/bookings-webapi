using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public class City : Entity
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public string State { get; set; } = string.Empty;

    public virtual List<Hotel>? Hotels { get; set; }
}