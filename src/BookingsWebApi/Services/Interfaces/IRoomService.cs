using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services.Interfaces;

/// <summary>
///     Service for managing rooms in the application.
/// </summary>
public interface IRoomService
{
    /// <summary>
    ///     Adds a new room to the database.
    /// </summary>
    /// <param name="dto">The data to create a new room.</param>
    /// <param name="roomHotel">The hotel associated with the room.</param>
    /// <returns>A <see cref="RoomModel" /> representing the newly created room.</returns>
    Task<RoomModel> AddRoom(RoomInsertDto dto, HotelModel roomHotel);

    /// <summary>
    ///     Deletes a booking with the given ID from the database.
    /// </summary>
    /// <param name="room">The entity that will be removed from the database.</param>
    Task DeleteRoom(RoomModel room);

    /// <summary>
    ///     Retrieves all rooms from the database.
    /// </summary>
    /// <returns>A list of <see cref="RoomModel" /> representing the rooms data.</returns>
    Task<List<RoomModel>> GetRooms();

    /// <summary>
    ///     Retrieves a hotel with the given ID from the database.
    /// </summary>
    /// <param name="hotelId">The hotel ID to search the database.</param>
    /// <returns>A <see cref="HotelModel" /> representing the hotel found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a hotel with the given ID does not exist.
    /// </exception>
    Task<HotelModel?> GetHotelById(Guid? hotelId);

    /// <summary>
    ///     Retrieves a room with the given ID from the database.
    /// </summary>
    /// <param name="id">The room ID to search the database.</param>
    /// <returns>A <see cref="RoomModel" /> representing the room found. </returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a room with the given ID does not exist.
    /// </exception>
    Task<RoomModel?> GetRoomById(Guid? id);

    /// <summary>
    ///     Updates the room with given ID in the database based on the provided data.
    /// </summary>
    /// <param name="dto">The data to update the room.</param>
    /// <param name="room">The entity that will be updated in the database.</param>
    /// <param name="roomHotel">The hotel associated with the room.</param>
    /// <returns>A <see cref="RoomModel" /> representing the updated room.</returns>
    Task<RoomModel> UpdateRoom(RoomInsertDto dto, RoomModel room, HotelModel roomHotel);
}
