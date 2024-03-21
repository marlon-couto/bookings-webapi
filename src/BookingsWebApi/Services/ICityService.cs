using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing cities in the application.
/// </summary>
public interface ICityService
{
    /// <summary>
    ///     Adds a new city to the database.
    /// </summary>
    /// <param name="dto">The data to create a new city.</param>
    /// <returns>A <see cref="CityModel" /> representing the newly created city.</returns>
    Task<CityModel> AddCity(CityInsertDto dto);

    /// <summary>
    ///     Deletes a city with the given ID from the database.
    /// </summary>
    /// <param name="city">The entity that will be removed from the database.</param>
    Task DeleteCity(CityModel city);

    /// <summary>
    ///     Retrieves all cities from the database.
    /// </summary>
    /// <returns>A list of <see cref="CityModel" /> representing the cities.</returns>
    Task<List<CityModel>> GetCities();

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>The <see cref="CityModel" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    Task<CityModel> GetCityById(string id);

    /// <summary>
    ///     Updates the city with the given ID in the database using the provided data.
    /// </summary>
    /// <param name="dto">The data to update the entity.</param>
    /// <param name="city">The entity that will be updated in the database.</param>
    /// <returns>A <see cref="CityModel" /> representing the updated city.</returns>
    Task<CityModel> UpdateCity(CityInsertDto dto, CityModel city);
}