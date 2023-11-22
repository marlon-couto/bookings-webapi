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

    public async Task<City> AddCity(CityInsertDto inputData)
    {
        City city = new() { CityId = Guid.NewGuid().ToString(), Name = inputData.Name, State = inputData.State };

        await _context.Cities.AddAsync(city);
        _context.SaveChanges();

        return city;
    }

    public void DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        _context.SaveChanges();
    }

    public async Task<List<City>> GetAllCities()
    {
        return await _context.Cities.ToListAsync();
    }

    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id)
               ?? throw new KeyNotFoundException("The city with the id provided does not exist");
    }

    public City UpdateCity(CityInsertDto inputData, City city)
    {
        city.Name = inputData.Name;
        city.State = inputData.State;
        _context.SaveChanges();

        return city;
    }
}