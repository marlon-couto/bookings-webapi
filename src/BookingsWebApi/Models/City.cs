using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public sealed class City : Entity
{
    [Required] [MinLength(2)] public string Name { get; set; } = string.Empty;

    [Required] public string State { get; set; } = string.Empty;

    public IEnumerable<Hotel>? Hotels { get; set; }
}