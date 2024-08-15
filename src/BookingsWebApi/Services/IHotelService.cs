using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing hotels in the application.
/// </summary>
public interface IHotelService
{
    /// <summary>
    ///     Adds a new hotel to the database.
    /// </summary>
    /// <param name="dto">The data to create a new hotel.</param>
    /// <param name="hotelCity">The city associated with the hotel.</param>
    /// <returns>A <see cref="HotelModel" /> representing the newly created hotel.</returns>
    Task<HotelModel> AddHotel(HotelInsertDto dto, CityModel hotelCity);

    /// <summary>
    ///     Deletes a hotel with the given ID from the database.
    /// </summary>
    /// <param name="hotel">The entity that will be removed.</param>
    Task DeleteHotel(HotelModel hotel);

    /// <summary>
    ///     Retrieves a list of hotels from the database.
    /// </summary>
    /// <returns>A list of <see cref="HotelModel" /> representing the hotels found.</returns>
    Task<List<HotelModel>> GetHotels();

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>A <see cref="CityModel" /> representing the city found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    Task<CityModel?> GetCityById(Guid? id);

    /// <summary>
    ///     Retrieves a hotel with the given ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to search the database.</param>
    /// <returns>A <see cref="HotelModel" /> representing the hotel found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a hotel with the given ID does not exist.
    /// </exception>
    Task<HotelModel?> GetHotelById(Guid id);

    /// <summary>
    ///     Retrieves all rooms associated with the hotel ID from the database.
    /// </summary>
    /// <param name="id">The hotel ID to filter the rooms.</param>
    /// <returns>
    ///     A list of <see cref="RoomModel" /> representing the rooms associated with
    ///     the given hotel ID.
    /// </returns>
    Task<List<RoomModel>> GetHotelRooms(Guid id);

    /// <summary>
    ///     Updates a hotel in the database.
    /// </summary>
    /// <param name="dto">The data used to update the hotel.</param>
    /// <param name="hotel">The entity that will be updated.</param>
    /// <param name="hotelCity">The city associated with the entity.</param>
    /// <returns>A <see cref="HotelModel" /> representing the updated hotel.</returns>
    Task<HotelModel> UpdateHotel(HotelInsertDto dto, HotelModel hotel, CityModel? hotelCity);
}