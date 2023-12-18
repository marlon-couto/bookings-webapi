using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using BookingsWebApi.Helpers;
using BookingsWebApi.Services;

using FluentAssertions;

using Moq;

using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class GeolocationServiceTest
{
    private readonly Mock<IHttpClientWrapper> _httpClientMock;
    private readonly GeolocationService _service;

    public GeolocationServiceTest()
    {
        _httpClientMock = new Mock<IHttpClientWrapper>();
        _service = new GeolocationService(_httpClientMock.Object);
    }

    [Fact(DisplayName = "GetGeolocationStatus should return API status")]
    public async Task GetGeolocationStatus_ShouldReturnApiStatus()
    {
        _httpClientMock
            .Setup(h => h.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"message\":\"OK\"}") }
            );

        object? result = await _service.GetGeolocationStatus();

        result.Should().NotBeNull();
    }

    [Fact(
        DisplayName = "GetGeolocationStatus throw HttpRequestException if service is unavailable"
    )]
    public async Task GetGeoLocationStatus_ThrowHttpRequestException_IfServiceIsUnavailable()
    {
        _httpClientMock
            .Setup(h => h.GetAsync(It.IsAny<string>()))
            .ThrowsAsync(new HttpRequestException("Geolocation service is unavailable."));

        Func<Task<object?>> act = () => _service.GetGeolocationStatus();

        await act.Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Geolocation service is unavailable.");
    }
}