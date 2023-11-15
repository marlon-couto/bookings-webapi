using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface ICityRepository
    {
        Task<List<CityDto>> GetAllCities();
        Task<CityDto> AddCity(CityInsertDto cityInsert);
        Task<City?> GetCityById(string cityId);
        CityDto UpdateCity(CityInsertDto cityInsert, City cityFound);
    }
}
