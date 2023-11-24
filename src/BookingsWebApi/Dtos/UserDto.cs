namespace BookingsWebApi.DTOs;

public record UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public record UserInsertDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public record LoginInsertDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}