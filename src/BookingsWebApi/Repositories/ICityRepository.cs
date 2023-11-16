using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories
{
    public interface ICityRepository
    {
        public Task<CityDto> AddCity(CityInsertDto inputData);
        public void DeleteCity(City city);
        public Task<List<CityDto>> GetAllCities();
        public Task<City> GetCityById(string id);
        public CityDto UpdateCity(City city, CityInsertDto inputData);
    }
}
