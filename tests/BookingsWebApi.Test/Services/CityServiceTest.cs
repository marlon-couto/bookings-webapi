using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Builders;
using BookingsWebApi.Test.Context;
using BookingsWebApi.Test.Helpers;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace BookingsWebApi.Test.Services;

public class CityServiceTest
{
    private readonly TestBookingsDbContext _context;
    private readonly Faker _faker = new();
    private readonly CityService _service;

    public CityServiceTest()
    {
        _context = TestUtils.CreateContext();
        _service = new CityService(_context);
    }

    [Fact(DisplayName = "AddCity should add city")]
    public async Task AddCity_ShouldAddCity()
    {
        CityInsertDto dto = new() { Name = _faker.Address.City(), State = _faker.Address.State() };
        City createdCity = await _service.AddCity(dto);

        createdCity.Should().NotBeNull();
        await _context.ClearDatabase(_context.Cities);
    }

    [Fact(DisplayName = "DeleteCity should remove city")]
    public async Task DeleteCity_ShouldRemoveCity()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        await _service.DeleteCity(city);

        List<City> allCities = await _context.Cities.ToListAsync();
        allCities.Count.Should().Be(0);
        await _context.ClearDatabase(_context.Cities);
    }

    [Fact(DisplayName = "GetAllCities should return all users")]
    public async Task GetAllCities_ShouldReturnAllCities()
    {
        City city1 = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city1);
        City city2 = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city2);
        await _context.SaveChangesAsync();

        List<City> allCities = await _service.GetAllCities();

        allCities.Count.Should().Be(2);
        await _context.ClearDatabase(_context.Cities);
    }

    [Fact(DisplayName = "GetCityById should return city found")]
    public async Task GetCityById_ShouldReturnCityFound()
    {
        City city = CityBuilder.New().Build();
        await _context.Cities.AddAsync(city);
        await _context.SaveChangesAsync();

        City cityFound = await _service.GetCityById(city.Id);

        cityFound.Should().NotBeNull();
        await _context.ClearDatabase(_context.Cities);
    }

    [Fact(DisplayName = "GetCityById throw KeyNotFoundException if city not exists")]
    public async Task GetCityById_ThrowKeyNotFoundException_IfCityNotExists()
    {
        Func<Task> act = async () => await _service.GetCityById(_faker.Random.Guid().ToString());

        await act.Should().ThrowAsync<KeyNotFoundException>()
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
        await _context.ClearDatabase(_context.Cities);
    }
}