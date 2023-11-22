using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly IBookingsDbContext _context;

    public HotelRepository(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Hotel> AddHotel(HotelInsertDto inputData, City hotelCity)
    {
        Hotel hotel = new()
        {
            HotelId = Guid.NewGuid().ToString(),
            Name = inputData.Name,
            CityId = inputData.CityId,
            Address = inputData.Address
        };

        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        hotel.City = hotelCity;
        return hotel;
    }

    public async Task DeleteHotel(Hotel hotel)
    {
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _context.Hotels.Include(h => h.City).ToListAsync();
    }

    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id)
               ?? throw new KeyNotFoundException("The city with the id provided does not exist");
    }

    public async Task<Hotel> GetHotelById(string id)
    {
        return await _context.Hotels
                   .Where(h => h.HotelId == id)
                   .Include(h => h.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The hotel with the id provided does not exist");
    }

    public async Task<List<Room>> GetHotelRooms(string id)
    {
        return await _context.Rooms.Where(r => r.HotelId == id).ToListAsync();
    }

    public async Task<Hotel> UpdateHotel(HotelInsertDto inputData, Hotel hotel, City hotelCity)
    {
        hotel.Name = inputData.Name;
        hotel.Address = inputData.Address;
        hotel.CityId = inputData.CityId;
        await _context.SaveChangesAsync();

        hotel.City = hotelCity;
        return hotel;
    }
}