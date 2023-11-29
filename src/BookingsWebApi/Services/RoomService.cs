using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing rooms in the application.
/// </summary>
public class RoomService
{
    private readonly IBookingsDbContext _context;

    public RoomService(IBookingsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Adds a new room to the database.
    /// </summary>
    /// <param name="dto">The data to create a new room.</param>
    /// <param name="roomHotel">The hotel associated with the room.</param>
    /// <returns>A <see cref="Room" /> representing the newly created room.</returns>
    public async Task<Room> AddRoom(RoomInsertDto dto, Hotel roomHotel)
    {
        Room room =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name,
                Image = dto.Image,
                HotelId = dto.HotelId,
                Capacity = dto.Capacity
            };

        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();

        room.Hotel = roomHotel;
        return room;
    }

    /// <summary>
    ///     Deletes a booking with the given ID from the database.
    /// </summary>
    /// <param name="room">The entity that will be removed from the database.</param>
    public async Task DeleteRoom(Room room)
    {
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Retrieves all rooms from the database.
    /// </summary>
    /// <returns>A list of <see cref="Room" /> representing the rooms data.</returns>
    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.Rooms.Include(r => r.Hotel).ThenInclude(h => h!.City).ToListAsync();
    }

    /// <summary>
    ///     Retrieves a hotel with the given ID from the database.
    /// </summary>
    /// <param name="hotelId">The hotel ID to search the database.</param>
    /// <returns>A <see cref="Hotel" /> representing the hotel found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a hotel with the given ID does not exist.
    /// </exception>
    public async Task<Hotel> GetHotelById(string hotelId)
    {
        return await _context
                   .Hotels
                   .Where(h => h.Id == hotelId)
                   .Include(h => h.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The hotel with the provided id does not exist.");
    }

    /// <summary>
    ///     Retrieves a room with the given ID from the database.
    /// </summary>
    /// <param name="id">The room ID to search the database.</param>
    /// <returns>A <see cref="Room" /> representing the room found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a room with the given ID does not exist.
    /// </exception>
    public async Task<Room> GetRoomById(string id)
    {
        return await _context
                   .Rooms
                   .Where(r => r.Id == id)
                   .Include(r => r.Hotel)
                   .ThenInclude(h => h!.City)
                   .FirstOrDefaultAsync()
               ?? throw new KeyNotFoundException("The room with the provided id does not exist.");
    }

    /// <summary>
    ///     Updates the room with given ID in the database based on the provided data.
    /// </summary>
    /// <param name="dto">The data to update the room.</param>
    /// <param name="room">The entity that will be updated in the database.</param>
    /// <param name="roomHotel">The hotel associated with the room.</param>
    /// <returns>A <see cref="Room" /> representing the updated room.</returns>
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