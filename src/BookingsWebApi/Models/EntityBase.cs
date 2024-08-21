using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public abstract class EntityBase
{
    [Key]
    public Guid Id { get; init; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.Now.ToUniversalTime();

    [Required]
    public bool IsDeleted { get; set; } = false;
}
