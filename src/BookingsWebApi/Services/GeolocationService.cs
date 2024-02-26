using System.Globalization;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services;

public class GeolocationService : IGeolocationService
{
    private const string StatusUrl = "https://nominatim.openstreetmap.org/status?format=json";
    private readonly IHttpClientWrapper _httpClient;

    public GeolocationService(IHttpClientWrapper httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<object?> GetGeolocationStatus()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(StatusUrl);
            response.EnsureSuccessStatusCode();

            object? result = await response.Content.ReadFromJsonAsync<object>();
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
        GeolocationJsonResponseDto? baseGeo = await GetGeolocation(dto);

        List<Hotel> hotels = await hotelService.GetHotels();
        GeolocationHotelDto[] hotelsGeolocations = await Task.WhenAll(
            hotels.Select(async h =>
            {
                GeolocationJsonResponseDto? hotelGeo = await GetGeolocation(
                    new GeolocationDto
                    {
                        Address = h.Address,
                        State = h.City!.State,
                        City = h.City.Name
                    }
                );

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

    private static int CalculateDistance(
        GeolocationJsonResponseDto origin,
        GeolocationJsonResponseDto destiny
    )
    {
        double originLatitude = double.Parse(origin.lat.Replace('.', ','));
        double originLongitude = double.Parse(origin.lon.Replace('.', ','));

        double destinyLatitude = double.Parse(destiny.lat.Replace('.', ','));
        double destinyLongitude = double.Parse(destiny.lon.Replace('.', ','));

        const double earthRadius = 6371;

        double diffLat = CalculateRadian(destinyLatitude - originLatitude);
        double diffLon = CalculateRadian(destinyLongitude - originLongitude);

        // Haversine formulae
        double a =
            (Math.Sin(diffLat / 2) * Math.Sin(diffLat / 2))
            + (
                Math.Cos(CalculateRadian(originLatitude))
                * Math.Cos(CalculateRadian(destinyLatitude))
                * Math.Sin(diffLon / 2)
                * Math.Sin(diffLon / 2)
            );

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = earthRadius * c;

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
            HttpResponseMessage response = await _httpClient.GetAsync(UriBuilder(dto));
            response.EnsureSuccessStatusCode();

            GeolocationJsonResponseDto? result =
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
