using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class CityService : ICityService
{
    private readonly IBookingsDbContext _ctx;

    public CityService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<CityModel?> AddCity(CityInsertDto dto)
    {
        var cityCreated = new CityModel
        {
            Id = Guid.NewGuid(),
            Name = dto.Name ?? string.Empty,
            State = dto.State ?? string.Empty
        };
        await _ctx.Cities.AddAsync(cityCreated);
        await _ctx.SaveChangesAsync();
        return cityCreated;
    }

    public async Task DeleteCity(CityModel city)
    {
        _ctx.Cities.Remove(city);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<CityModel>> GetCities()
    {
        return await _ctx.Cities.AsNoTracking().ToListAsync();
    }

    public async Task<CityModel?> GetCityById(Guid id)
    {
        return await _ctx.Cities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<CityModel> UpdateCity(CityInsertDto dto, CityModel city)
    {
        city.Name = dto.Name ?? string.Empty;
        city.State = dto.State ?? string.Empty;
        await _ctx.SaveChangesAsync();
        return city;
    }
}
