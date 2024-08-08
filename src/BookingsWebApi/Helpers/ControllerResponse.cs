namespace BookingsWebApi.Helpers;

public enum EResult
{
    Succeeded,
    Failed
}

public class ControllerResponse
{
    public EResult Result { get; set; } = EResult.Succeeded;
    public string? Message { get; set; }
    public object? Data { get; init; }
}