namespace BookingsWebApi.Helpers;

public interface IHttpClientWrapper
{
    Task<HttpResponseMessage> GetAsync(string requestUri);
}