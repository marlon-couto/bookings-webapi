using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public abstract class Entity
{
    [Required] public string Id { get; set; } = string.Empty;
}