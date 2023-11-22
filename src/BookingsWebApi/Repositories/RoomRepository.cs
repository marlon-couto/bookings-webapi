using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly IBookingsDbContext _context;

    public RoomRepository(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<Room> AddRoom(RoomInsertDto inputData, Hotel roomHotel)
    {
        Room room = new()
        {
            RoomId = Guid.NewGuid().ToString(),
            Name = inputData.Name,
            Image = inputData.Image,
            HotelId = inputData.HotelId,
            Capacity = inputData.Capacity
        };

        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }

    public async Task DeleteRoom(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.Rooms
            .Include(r => r.Hotel)
            .Include(r => r.Hotel!.City)
            .ToListAsync();
    }

    public async Task<Hotel> GetHotelById(string hotelId)
    {
        return await _context.Hotels
                   .Where(h => h.HotelId == hotelId)
                   .Include(h => h.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The hotel with the provided id does not exist");
    }

    public async Task<Room> GetRoomById(string id)
    {
        return await _context.Rooms
                   .Where(r => r.RoomId == id)
                   .Include(r => r.Hotel)
                   .Include(r => r.Hotel!.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The room with the provided id does not exist");
    }

    public async Task<Room> UpdateRoom(RoomInsertDto inputData, Room room, Hotel roomHotel)
    {
        room.Capacity = inputData.Capacity;
        room.HotelId = inputData.HotelId;
        room.Image = inputData.Image;
        room.Name = inputData.Name;
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }
}