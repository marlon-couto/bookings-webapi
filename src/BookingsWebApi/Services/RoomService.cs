using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class RoomService : IRoomService
{
    private readonly IBookingsDbContext _context;

    public RoomService(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Room> AddRoom(RoomInsertDto dto, Hotel roomHotel)
    {
        Room roomCreated =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Image = dto.Image,
                HotelId = dto.HotelId,
                Capacity = dto.Capacity
            };

        await _context.Rooms.AddAsync(roomCreated);
        await _context.SaveChangesAsync();

        roomCreated.Hotel = roomHotel;
        return roomCreated;
    }

    public async Task DeleteRoom(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetRooms()
    {
        List<Room> rooms = await _context
            .Rooms.AsNoTracking()
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .ToListAsync();

        return rooms;
    }

    public async Task<Hotel> GetHotelById(string hotelId)
    {
        Hotel? hotelFound = await _context
            .Hotels.Where(h => h.Id == hotelId)
            .Include(h => h.City)
            .FirstOrDefaultAsync();

        return hotelFound
            ?? throw new KeyNotFoundException("The hotel with the provided id does not exist.");
    }

    public async Task<Room> GetRoomById(string id)
    {
        Room? roomFound = await _context
            .Rooms.Where(r => r.Id == id)
            .Include(r => r.Hotel)
            .ThenInclude(h => h!.City)
            .FirstOrDefaultAsync();

        return roomFound
            ?? throw new KeyNotFoundException("The room with the provided id does not exist.");
    }

    public async Task<Room> UpdateRoom(RoomInsertDto dto, Room room, Hotel roomHotel)
    {
        room.Capacity = dto.Capacity;
        room.HotelId = dto.HotelId;
        room.Image = dto.Image;
        room.Name = dto.Name;
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }
}
