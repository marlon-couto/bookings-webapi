using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories;

public interface IHotelRepository
{
    public Task<HotelDto> AddHotel(HotelInsertDto inputData, City city);
    public void DeleteHotel(Hotel hotel);
    public Task<List<HotelDto>> GetAllHotels();
    public Task<City> GetCityById(string id);
    public Task<Hotel> GetHotelById(string id);
    public HotelDto UpdateHotel(Hotel hotel, City city, HotelInsertDto inputData);
}
