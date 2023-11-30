using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public abstract class Entity
{
    [Key]
    public string Id { get; init; } = string.Empty;
}
