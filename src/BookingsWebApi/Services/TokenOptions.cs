namespace BookingsWebApi.Services;

public class TokenOptions
{
    public const string Token = "Token";
    public string Secret { get; init; } = string.Empty;
    public int ExpiresDay { get; init; }
}
