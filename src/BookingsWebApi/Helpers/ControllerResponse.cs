using System.Text.Json.Serialization;

namespace BookingsWebApi.Helpers;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EResult
{
    Succeeded,
    Failed
}

public class ControllerResponse
{
    public EResult Result { get; set; } = EResult.Succeeded;
    public int StatusCode { get; set; } = 200;
    public string? Message { get; set; } = "OK";
    public object? Data { get; set; }
}
