using BookingsWebApi.DTOs;

namespace BookingsWebApi.Services.Interfaces;

public interface IGeolocationService
{
    Task<object?> GetGeolocationStatus();

    Task<List<GeolocationHotelDto>> GetHotelsByGeolocation(
        GeolocationDto dto,
        IHotelService hotelService
    );
}