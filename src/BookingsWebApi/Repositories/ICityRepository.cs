using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface ICityRepository
    {
        public Task<List<CityDto>> GetAllCities();
        public Task<CityDto> AddCity(CityInsertDto inputData);
        public Task<City?> GetCityById(string id);
        public CityDto UpdateCity(CityInsertDto inputData, City cityFound);
    }
}
