using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing hotels in the application.
/// </summary>
public class HotelService
{
    private readonly IBookingsDbContext _context;

    public HotelService(IBookingsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Adds a new hotel to the database.
    /// </summary>
    /// <param name="dto">The data to create a new hotel.</param>
    /// <param name="hotelCity">The city associated with the hotel.</param>
    /// <returns>A <see cref="Hotel" /> representing the newly created hotel.</returns>
    public async Task<Hotel> AddHotel(HotelInsertDto dto, City hotelCity)
    {
        Hotel hotel =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                CityId = dto.CityId,
                Address = dto.Address
            };

        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();

        hotel.City = hotelCity;
        return hotel;
    }

    /// <summary>
    ///     Deletes a hotel with the given ID from the database.
    /// </summary>
    /// <param name="hotel">The entity that will be removed.</param>
    public async Task DeleteHotel(Hotel hotel)
    {
        _context.Hotels.Remove(hotel);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves a list of hotels from the database.
    /// </summary>
    /// <returns>A list of <see cref="Hotel" /> representing the hotels found.</returns>
    public async Task<List<Hotel>> GetAllHotels()
    {
        return await _context.Hotels.AsNoTracking().Include(h => h.City).ToListAsync();
    }

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>A <see cref="City" /> representing the city found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new KeyNotFoundException("The city with the id provided does not exist.");
    }

    /// <summary>
    ///     Retrieves a hotel with the given ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to search the database.</param>
    /// <returns>A <see cref="Hotel" /> representing the hotel found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a hotel with the given ID does not exist.
    /// </exception>
    public async Task<Hotel> GetHotelById(string id)
    {
        return await _context
                .Hotels
                .Where(h => h.Id == id)
                .Include(h => h.City)
                .FirstOrDefaultAsync()
            ?? throw new KeyNotFoundException("The hotel with the id provided does not exist.");
    }

    /// <summary>
    ///     Retrieves all rooms associated with the hotel ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to filter the rooms.</param>
    /// <returns>
    ///     A list of <see cref="Room" /> representing the rooms associated with
    ///     the given hotel ID.
    /// </returns>
    public async Task<List<Room>> GetHotelRooms(string id)
    {
        return await _context.Rooms.Where(r => r.HotelId == id).ToListAsync();
    }

    /// <summary>
    ///     Updates a hotel in the database.
    /// </summary>
    /// <param name="dto">The data used to update the hotel.</param>
    /// <param name="hotel">The entity that will be updated.</param>
    /// <param name="hotelCity">The city associated with the entity.</param>
    /// <returns>A <see cref="Hotel" /> representing the updated hotel.</returns>
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
