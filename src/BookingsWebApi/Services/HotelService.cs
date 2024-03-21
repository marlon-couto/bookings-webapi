using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class HotelService : IHotelService
{
    private readonly IBookingsDbContext _context;

    public HotelService(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Hotel> AddHotel(HotelInsertDto dto, City hotelCity)
    {
        Hotel hotelCreated =
            new()
            {
                Id = Guid.NewGuid().ToString(), Name = dto.Name, CityId = dto.CityId, Address = dto.Address
            };

        await _context.Hotels.AddAsync(hotelCreated);
        await _context.SaveChangesAsync();

        hotelCreated.City = hotelCity;
        return hotelCreated;
    }

    public async Task DeleteHotel(Hotel hotel)
    {
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Hotel>> GetHotels()
    {
        List<Hotel> hotels = await _context
            .Hotels.AsNoTracking()
            .Include(h => h.City)
            .ToListAsync();
        return hotels;
    }

    public async Task<City> GetCityById(string id)
    {
        City? cityFound = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        return cityFound
               ?? throw new KeyNotFoundException("The city with the id provided does not exist.");
    }

    public async Task<Hotel> GetHotelById(string id)
    {
        Hotel? hotelFound = await _context
            .Hotels.Where(h => h.Id == id)
            .Include(h => h.City)
            .FirstOrDefaultAsync();

        return hotelFound
               ?? throw new KeyNotFoundException("The hotel with the id provided does not exist.");
    }

    public async Task<List<Room>> GetHotelRooms(string id)
    {
        List<Room> hotelRooms = await _context.Rooms.Where(r => r.HotelId == id).ToListAsync();
        return hotelRooms;
    }

    public async Task<Hotel> UpdateHotel(HotelInsertDto dto, Hotel hotel, City hotelCity)
    {
        hotel.Name = dto.Name;
        hotel.Address = dto.Address;
        hotel.CityId = dto.CityId;
        await _context.SaveChangesAsync();

        hotel.City = hotelCity;
        return hotel;
    }
}