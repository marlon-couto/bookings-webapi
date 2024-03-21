using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class CityService : ICityService
{
    private readonly IBookingsDbContext _context;

    public CityService(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<CityModel> AddCity(CityInsertDto dto)
    {
        CityModel cityCreated =
            new()
            {
                Id = Guid.NewGuid().ToString(), Name = dto.Name, State = dto.State
            };

        await _context.Cities.AddAsync(cityCreated);
        await _context.SaveChangesAsync();

        return cityCreated;
    }

    public async Task DeleteCity(CityModel city)
    {
        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();
    }

    public async Task<List<CityModel>> GetCities()
    {
        List<CityModel> cities = await _context.Cities.AsNoTracking().ToListAsync();
        return cities;
    }

    public async Task<CityModel> GetCityById(string id)
    {
        CityModel? cityFound = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
        return cityFound
               ?? throw new KeyNotFoundException("The city with the id provided does not exist.");
    }

    public async Task<CityModel> UpdateCity(CityInsertDto dto, CityModel city)
    {
        city.Name = dto.Name;
        city.State = dto.State;
        await _context.SaveChangesAsync();

        return city;
    }
}