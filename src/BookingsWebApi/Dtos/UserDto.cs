namespace BookingsWebApi.DTOs;

public record UserDto
{
    public Guid? Id { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Role { get; init; }
}

public record UserInsertDto
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Password { get; init; }
}

public record LoginInsertDto
{
    public string? Email { get; init; }
    public string? Password { get; init; }
}