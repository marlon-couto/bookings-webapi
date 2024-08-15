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

public class HotelServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly HotelService _service;

    public HotelServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new HotelService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddHotel should add hotel")]
    public async Task AddHotel_ShouldAddHotel()
    {
        var hotelCity = CityBuilder.New().Build();
        var dto = new HotelInsertDto
        {
            Name = _faker.Lorem.Sentence(), Address = _faker.Address.FullAddress(), CityId = hotelCity.Id
        };
        var hotelCreated = await _service.AddHotel(dto, hotelCity);
        hotelCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteHotel should delete hotel")]
    public async Task DeleteHotel_ShouldDeleteHotel()
    {
        var hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        await _service.DeleteHotel(hotel);
        var hotels = await _context.Hotels.AsNoTracking().ToListAsync();
        hotels.Count.Should().Be(0);
    }

    [Fact(DisplayName = "GetHotels should return all hotels")]
    public async Task GetHotels_ShouldReturnAllHotels()
    {
        var hotel1 = HotelBuilder.New().Build();
        var hotel2 = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel1);
        await _context.Hotels.AddAsync(hotel2);
        await _context.SaveChangesAsync();
        var hotels = await _service.GetHotels();
        hotels.Count.Should().Be(2);
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

    [Fact(DisplayName = "GetHotelId should return hotel found")]
    public async Task GetHotelById_ShouldReturnHotelFound()
    {
        var hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        var hotelFound = await _service.GetHotelById(hotel.Id);
        hotelFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetHotelById return null if hotel not exists")]
    public async Task GetHotelById_ReturnNull_IfHotelNotExists()
    {
        var hotelFound = await _service.GetHotelById(_faker.Random.Guid());
        hotelFound.Should().BeNull();
    }

    [Fact(DisplayName = "GetHotelRooms should return all hotel rooms")]
    public async Task GetHotelRooms_ShouldReturnAllHotelRooms()
    {
        var hotel = HotelBuilder.New().Build();
        var room1 = RoomBuilder.New().WithHotel(hotel).Build();
        var room2 = RoomBuilder.New().WithHotel(hotel).Build();
        await _context.Rooms.AddAsync(room1);
        await _context.Rooms.AddAsync(room2);
        await _context.SaveChangesAsync();
        var hotelRooms = await _service.GetHotelRooms(hotel.Id);
        hotelRooms.Count.Should().Be(2);
    }

    [Fact(DisplayName = "UpdateHotel should update hotel")]
    public async Task UpdateHotel_ShouldUpdateHotel()
    {
        var hotel = HotelBuilder.New().Build();
        await _context.Hotels.AddAsync(hotel);
        await _context.SaveChangesAsync();
        var dto = new HotelInsertDto
        {
            Address = _faker.Address.FullAddress(), CityId = hotel.CityId, Name = _faker.Lorem.Sentence()
        };
        var hotelUpdated = await _service.UpdateHotel(dto, hotel, hotel.City!);
        hotelUpdated.Should().NotBeNull();
    }
}