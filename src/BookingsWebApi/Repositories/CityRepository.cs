using AutoMapper;

using BookingsWebApi.Dtos;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class CityRepository : ICityRepository
{
    private readonly IBookingsDbContext _context;
    private readonly IMapper _mapper;
    public CityRepository(IBookingsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CityDto> AddCity(CityInsertDto inputData)
    {
        City newCity = _mapper.Map<City>(inputData);
        newCity.CityId = Guid.NewGuid().ToString();

        await _context.Cities.AddAsync(newCity);
        _context.SaveChanges();

        return _mapper.Map<CityDto>(newCity);
    }

    public void DeleteCity(City city)
    {
        _context.Cities.Remove(city);
        _context.SaveChanges();
    }

    public async Task<List<CityDto>> GetAllCities()
    {
        return await _context.Cities.Select(c => _mapper.Map<CityDto>(c)).ToListAsync();
    }

    public async Task<City> GetCityById(string id)
    {
        return await _context.Cities.FirstOrDefaultAsync(c => c.CityId == id)
            ?? throw new KeyNotFoundException("The city with the id provided does not exist");
    }

    public CityDto UpdateCity(City city, CityInsertDto inputData)
    {
        city.Name = inputData.Name;
        city.State = inputData.State;
        _context.SaveChanges();

        return _mapper.Map<CityDto>(city);
    }
}
