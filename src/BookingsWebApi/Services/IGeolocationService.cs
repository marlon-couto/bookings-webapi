using BookingsWebApi.DTOs;

namespace BookingsWebApi.Services;

public interface IGeolocationService
{
    Task<object?> GetGeolocationStatus();

    Task<List<GeolocationHotelDto>> GetHotelsByGeolocation(
        GeolocationDto dto,
        IHotelService hotelService
    );
}
