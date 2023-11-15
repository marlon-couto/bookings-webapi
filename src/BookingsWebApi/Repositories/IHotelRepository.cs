using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface IHotelRepository
    {
        public Task<List<HotelDto>> GetAllHotels();
        public Task<City?> GetCityById(string id);
        public Task<HotelDto> AddHotel(HotelInsertDto inputData, City cityFound);
    }
}
