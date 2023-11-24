using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories;

/// <summary>
///     Repository for managing hotels in the application.
/// </summary>
public interface IHotelRepository
{
    /// <summary>
    ///     Adds a new hotel to the database.
    /// </summary>
    /// <param name="inputData">The data to create a new hotel.</param>
    /// <param name="hotelCity">The city associated with the hotel.</param>
    /// <returns>A <see cref="Hotel" /> representing the newly created hotel.</returns>
    Task<Hotel> AddHotel(HotelInsertDto inputData, City hotelCity);

    /// <summary>
    ///     Deletes a hotel with the given ID from the database.
    /// </summary>
    /// <param name="hotel">The entity that will be removed.</param>
    Task DeleteHotel(Hotel hotel);

    /// <summary>
    ///     Retrieves a list of hotels from the database.
    /// </summary>
    /// <returns>A list of <see cref="Hotel" /> representing the hotels found.</returns>
    Task<List<Hotel>> GetAllHotels();

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>A <see cref="City" /> representing the city found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    Task<City> GetCityById(string id);

    /// <summary>
    ///     Retrieves a hotel with the given ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to search the database.</param>
    /// <returns>A <see cref="Hotel" /> representing the hotel found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a hotel with the given ID does not exist.
    /// </exception>
    Task<Hotel> GetHotelById(string id);

    /// <summary>
    ///     Retrieves all rooms associated with the hotel ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to filter the rooms.</param>
    /// <returns>A list of <see cref="Room" /> representing the rooms associated with the given hotel ID.</returns>
    Task<List<Room>> GetHotelRooms(string id);

    /// <summary>
    ///     Updates a hotel in the database.
    /// </summary>
    /// <param name="inputData">The data used to update the hotel.</param>
    /// <param name="hotel">The entity that will be updated.</param>
    /// <param name="hotelCity">The city associated with the entity.</param>
    /// <returns>A <see cref="Hotel" /> representing the updated hotel.</returns>
    Task<Hotel> UpdateHotel(HotelInsertDto inputData, Hotel hotel, City hotelCity);
}