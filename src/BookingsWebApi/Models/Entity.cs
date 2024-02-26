using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public abstract class Entity
{
    [Key]
    [MaxLength(16)]
    public string Id { get; init; } = string.Empty;
}
