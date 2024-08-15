using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services.Interfaces;
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
            Id = Guid.NewGuid(),
            Name = dto.Name ?? string.Empty,
            CityId = dto.CityId ?? Guid.Empty,
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
        return await _ctx.Hotels.AsNoTracking()
            .Include(h => h.City)
            .ToListAsync();
    }

    public async Task<CityModel?> GetCityById(Guid? id)
    {
        return await _ctx.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id) ?? null;
    }

    public async Task<HotelModel?> GetHotelById(Guid id)
    {
        return await _ctx.Hotels.AsNoTracking()
            .Where(h => h.Id == id)
            .Include(h => h.City)
            .FirstOrDefaultAsync();
    }

    public async Task<List<RoomModel>> GetHotelRooms(Guid id)
    {
        return await _ctx.Rooms.AsNoTracking().Where(r => r.HotelId == id).ToListAsync();
    }

    public async Task<HotelModel> UpdateHotel(HotelInsertDto dto, HotelModel hotel, CityModel? hotelCity)
    {
        hotel.Name = dto.Name ?? string.Empty;
        hotel.Address = dto.Address ?? string.Empty;
        hotel.CityId = dto.CityId ?? Guid.Empty;
        await _ctx.SaveChangesAsync();
        hotel.City = hotelCity;
        return hotel;
    }
}