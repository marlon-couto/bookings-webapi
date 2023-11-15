using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface ICityRepository
    {
        Task<List<CityDto>> GetAllCities();
        Task<CityDto> AddCity(CityInsertDto inputData);
        Task<City?> GetCityById(string id);
        CityDto UpdateCity(CityInsertDto inputData, City cityFound);
    }
}
