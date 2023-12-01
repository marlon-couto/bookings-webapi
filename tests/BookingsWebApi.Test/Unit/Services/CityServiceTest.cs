using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
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
        CityInsertDto dto = new() { Name = _faker.Address.City(), State = _faker.Address.State() };
        City cityCreated = await _service.AddCity(dto);

        cityCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteCity should remove city")]
    public async Task DeleteCity_ShouldRemoveCity()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        await _service.DeleteCity(city);

        List<City> cities = await _context.Cities.AsNoTracking().ToListAsync();
        cities.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetCities should return all cities")]
    public async Task GetCities_ShouldReturnAllCities()
    {
        City city1 = CityBuilder.New().Build();
        City city2 = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city1);
        await _context.Cities.AddAsync(city2);
        await _context.SaveChangesAsync();

        List<City> cities = await _service.GetCities();

        cities.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetCityById should return city found")]
    public async Task GetCityById_ShouldReturnCityFound()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        City cityFound = await _service.GetCityById(city.Id);

        cityFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetCityById throw KeyNotFoundException if city not exists")]
    public async Task GetCityById_ThrowKeyNotFoundException_IfCityNotExists()
    {
        Func<Task> act = async () => await _service.GetCityById(_faker.Random.Guid().ToString());

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("The city with the id provided does not exist.");
    }

    [Fact(DisplayName = "UpdateCity should update city")]
    public async Task UpdateCity_ShouldUpdateCity()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        CityInsertDto dto = new() { Name = _faker.Address.City(), State = _faker.Address.State() };
        City cityUpdated = await _service.UpdateCity(dto, city);

        cityUpdated.Should().NotBeNull();
    }
}