using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IHotelRepository
    {
        Task<List<HotelDto>> GetAllHotels();
        Task<City?> GetCityById(string cityId);
        Task<HotelDto> AddHotel(HotelInsertDto hotelInsert, City cityFound);
    }
}
