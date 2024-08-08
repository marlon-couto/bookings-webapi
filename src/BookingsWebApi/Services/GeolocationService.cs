using System.Globalization;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;

namespace BookingsWebApi.Services;

public class GeolocationService : IGeolocationService
{
    private const string StatusUrl = "https://nominatim.openstreetmap.org/status?format=json";
    private readonly IHttpClientWrapper _client;

    public GeolocationService(IHttpClientWrapper client)
    {
        _client = client;
    }

    public async Task<object?> GetGeolocationStatus()
    {
        try
        {
            var response = await _client.GetAsync(StatusUrl);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<object>();
            return result;
        }
        catch (HttpRequestException)
        {
            throw new HttpRequestException("Geolocation service is unavailable.");
        }
    }

    public async Task<List<GeolocationHotelDto>> GetHotelsByGeolocation(
        GeolocationDto dto,
        IHotelService hotelService
    )
    {
        var baseGeo = await GetGeolocation(dto);
        var hotels = await hotelService.GetHotels();
        var hotelsGeolocations = await Task.WhenAll(
            hotels.Select(async h =>
            {
                var geolocationDto
                    = new GeolocationDto { Address = h.Address, State = h.City!.State, City = h.City.Name };
                var hotelGeo = await GetGeolocation(geolocationDto);
                return new GeolocationHotelDto
                {
                    Name = h.Address,
                    Address = h.Address,
                    CityName = h.City!.Name,
                    Id = h.Id,
                    State = h.City.State,
                    Distance = CalculateDistance(baseGeo!, hotelGeo!)
                };
            })
        );
        return hotelsGeolocations.OrderBy(h => h.Distance).ThenBy(h => h.Name).ToList();
    }

    private static int? CalculateDistance(
        GeolocationJsonResponseDto origin,
        GeolocationJsonResponseDto destiny
    )
    {
        if (origin.lat == null || origin.lon == null || destiny.lat == null || destiny.lon == null)
        {
            return null;
        }

        var originLatitude = double.Parse(origin.lat.Replace('.', ','));
        var originLongitude = double.Parse(origin.lon.Replace('.', ','));
        var destinyLatitude = double.Parse(destiny.lat.Replace('.', ','));
        var destinyLongitude = double.Parse(destiny.lon.Replace('.', ','));
        const double earthRadius = 6371;
        var diffLat = CalculateRadian(destinyLatitude - originLatitude);
        var diffLon = CalculateRadian(destinyLongitude - originLongitude);

        // Haversine formulae
        var a =
            (Math.Sin(diffLat / 2) * Math.Sin(diffLat / 2))
            + (
                Math.Cos(CalculateRadian(originLatitude))
                * Math.Cos(CalculateRadian(destinyLatitude))
                * Math.Sin(diffLon / 2)
                * Math.Sin(diffLon / 2)
            );
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = earthRadius * c;
        return int.Parse(Math.Round(distance, 0).ToString(CultureInfo.InvariantCulture));
    }

    private static double CalculateRadian(double degree)
    {
        return degree * Math.PI / 180;
    }

    private async Task<GeolocationJsonResponseDto?> GetGeolocation(GeolocationDto dto)
    {
        try
        {
            var response = await _client.GetAsync(UriBuilder(dto));
            response.EnsureSuccessStatusCode();
            var result =
                await response.Content.ReadFromJsonAsync<GeolocationJsonResponseDto>();
            return result != null
                ? new GeolocationJsonResponseDto { lat = result.lat, lon = result.lon }
                : null;
        }
        catch (HttpRequestException e)
        {
            throw new HttpRequestException(e.Message);
        }
    }

    private static string UriBuilder(GeolocationDto dto)
    {
        const string baseUrl = "https://nominatim.openstreetmap.org/search?";
        return $"{baseUrl}street={dto.Address}&city={dto.City}&country=Brazil&state={dto.State}&format=json&limit=1";
    }
}