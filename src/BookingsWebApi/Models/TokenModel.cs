namespace BookingsWebApi.Models;

public class TokenModel
{
    public string Secret { get; init; } = string.Empty;
    public int ExpireDay { get; init; }
}