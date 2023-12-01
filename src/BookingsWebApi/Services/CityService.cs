using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing cities in the application.
/// </summary>
public class CityService
{
    private readonly IBookingsDbContext _context;

    public CityService(IBookingsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Adds a new city to the database.
    /// </summary>
    /// <param name="dto">The data to create a new city.</param>
    /// <returns>A <see cref="City" /> representing the newly created city.</returns>
    public async Task<City> AddCity(CityInsertDto dto)
    {
        City city =
            new() { Id = Guid.NewGuid().ToString(), Name = dto.Name, State = dto.State };

        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        return city;
    }

    /// <summary>
    ///     Deletes a city with the given ID from the database.
    /// </summary>
    /// <param name="city">The entity that will be removed from the database.</param>
    public async Task DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all cities from the database.
    /// </summary>
    /// <returns>A list of <see cref="City" /> representing the cities.</returns>
    public async Task<List<City>> GetCities()
    {
        return await _context.Cities.AsNoTracking().ToListAsync();
    }

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>The <see cref="City" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id)
               ?? throw new KeyNotFoundException("The city with the id provided does not exist.");
    }

    /// <summary>
    ///     Updates the city with the given ID in the database using the provided data.
    /// </summary>
    /// <param name="dto">The data to update the entity.</param>
    /// <param name="city">The entity that will be updated in the database.</param>
    /// <returns>A <see cref="City" /> representing the updated city.</returns>
    public async Task<City> UpdateCity(CityInsertDto dto, City city)
    {
        city.Name = dto.Name;
        city.State = dto.State;
        await _context.SaveChangesAsync();

        return city;
    }
}