using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IHotelRepository
    {
        Task<List<HotelDto>> GetAllHotels();
        Task<City?> GetCityById(string id);
        Task<HotelDto> AddHotel(HotelInsertDto inputData, City cityFound);
    }
}
