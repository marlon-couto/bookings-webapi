namespace BookingsWebApi.Services;

public class TokenOptions
{
    public const string Token = "Token";
    public string Secret { get; set; } = string.Empty;
    public int ExpiresDay { get; set; }
}
