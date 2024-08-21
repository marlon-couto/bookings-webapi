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
            State = dto.State ?? string.Empty,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };
        await _ctx.Cities.AddAsync(cityCreated);
        await _ctx.SaveChangesAsync();
        return cityCreated;
    }

    public async Task DeleteCity(CityModel city)
    {
        city.IsDeleted = true;
        city.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<CityModel>> GetCities()
    {
        return await _ctx.Cities.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<CityModel?> GetCityById(Guid id)
    {
        return await _ctx.Cities.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<CityModel> UpdateCity(CityInsertDto dto, CityModel city)
    {
        city.Name = dto.Name ?? string.Empty;
        city.State = dto.State ?? string.Empty;
        city.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
        return city;
    }
}
