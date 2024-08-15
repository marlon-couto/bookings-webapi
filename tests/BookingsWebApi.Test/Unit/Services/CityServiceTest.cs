using System;
using System.Threading.Tasks;
using Bogus;
using BookingsWebApi.DTOs;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers;
using BookingsWebApi.Test.Helpers.Builders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class CityServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly CityService _service;

    public CityServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new CityService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddCity should add city")]
    public async Task AddCity_ShouldAddCity()
    {
        var dto = new CityInsertDto { Name = _faker.Address.City(), State = _faker.Address.State() };
        var cityCreated = await _service.AddCity(dto);
        cityCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteCity should remove city")]
    public async Task DeleteCity_ShouldRemoveCity()
    {
        var city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
        await _service.DeleteCity(city);
        var cities = await _context.Cities.AsNoTracking().ToListAsync();
        cities.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetCities should return all cities")]
    public async Task GetCities_ShouldReturnAllCities()
    {
        var city1 = CityBuilder.New().Build();
        var city2 = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city1);
        await _context.Cities.AddAsync(city2);
        await _context.SaveChangesAsync();
        var cities = await _service.GetCities();
        cities.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetCityById should return city found")]
    public async Task GetCityById_ShouldReturnCityFound()
    {
        var city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
        var cityFound = await _service.GetCityById(city.Id);
        cityFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetCityById return null if city not exists")]
    public async Task GetCityById_ReturnNull_IfCityNotExists()
    {
        var cityFound = await _service.GetCityById(_faker.Random.Guid());
        cityFound.Should().BeNull();
    }

    [Fact(DisplayName = "UpdateCity should update city")]
    public async Task UpdateCity_ShouldUpdateCity()
    {
        var city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();
        var dto = new CityInsertDto { Name = _faker.Address.City(), State = _faker.Address.State() };
        var cityUpdated = await _service.UpdateCity(dto, city);
        cityUpdated.Should().NotBeNull();
    }
}