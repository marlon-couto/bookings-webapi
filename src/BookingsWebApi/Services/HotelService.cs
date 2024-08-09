using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class HotelService : IHotelService
{
    private readonly IBookingsDbContext _ctx;

    public HotelService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<HotelModel> AddHotel(HotelInsertDto dto, CityModel hotelCity)
    {
        var hotelCreated = new HotelModel
        {
            Id = Guid.NewGuid().ToString(),
            Name = dto.Name ?? string.Empty,
            CityId = dto.CityId ?? string.Empty,
            Address = dto.Address ?? string.Empty
        };
        await _ctx.Hotels.AddAsync(hotelCreated);
        await _ctx.SaveChangesAsync();
        hotelCreated.City = hotelCity;
        return hotelCreated;
    }

    public async Task DeleteHotel(HotelModel hotel)
    {
        _ctx.Hotels.Remove(hotel);
        await _ctx.SaveChangesAsync();
    }

    public async Task<List<HotelModel>> GetHotels()
    {
        return await _ctx
            .Hotels.AsNoTracking()
            .Include(h => h.City)
            .ToListAsync();
    }

    public async Task<CityModel?> GetCityById(string? id)
    {
        return await _ctx.Cities.FirstOrDefaultAsync(c => c.Id == id) ?? null;
    }

    public async Task<HotelModel?> GetHotelById(string id)
    {
        return await _ctx
            .Hotels.Where(h => h.Id == id)
            .Include(h => h.City)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RoomModel>> GetHotelRooms(string id)
    {
        return await _ctx.Rooms.Where(r => r.HotelId == id).ToListAsync();
    }

    public async Task<HotelModel> UpdateHotel(HotelInsertDto dto, HotelModel hotel, CityModel? hotelCity)
    {
        hotel.Name = dto.Name ?? string.Empty;
        hotel.Address = dto.Address ?? string.Empty;
        hotel.CityId = dto.CityId ?? string.Empty;
        await _ctx.SaveChangesAsync();
        hotel.City = hotelCity;
        return hotel;
    }
}