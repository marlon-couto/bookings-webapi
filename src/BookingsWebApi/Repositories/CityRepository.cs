using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class CityRepository : ICityRepository
{
    private readonly IBookingsDbContext _context;

    public CityRepository(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<City> AddCity(CityInsertDto dto)
    {
        City city =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                State = dto.State
            };

        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        return city;
    }

    public async Task DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }

    public async Task<List<City>> GetAllCities()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.Id == id)
               ?? throw new KeyNotFoundException(
                   "The city with the id provided does not exist");
    }

    public async Task<City> UpdateCity(CityInsertDto dto, City city)
    {
        city.Name = dto.Name;
        city.State = dto.State;
        await _context.SaveChangesAsync();

        return city;
    }
}