using System.ComponentModel.DataAnnotations;

namespace BookingsWebApi.Models;

public abstract class EntityBase
{
    [Key] public Guid Id { get; init; }
}