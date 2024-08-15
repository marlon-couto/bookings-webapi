namespace BookingsWebApi.Models;

public class TokenModel
{
    public const string Token = "Token";
    public string Secret { get; init; } = string.Empty;
    public int ExpireDay { get; init; }
}