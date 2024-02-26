namespace BookingsWebApi.Helpers;

public class ControllerResponse<T>
{
    public T? Data { get; init; }
    public string Result { get; set; } = string.Empty;
}

public class ControllerErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
}
