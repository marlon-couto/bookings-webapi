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
    /// <returns>A <see cref="City" /> representing the newly created city.</returns>
    Task<City> AddCity(CityInsertDto dto);

    /// <summary>
    ///     Deletes a city with the given ID from the database.
    /// </summary>
    /// <param name="city">The entity that will be removed from the database.</param>
    Task DeleteCity(City city);

    /// <summary>
    ///     Retrieves all cities from the database.
    /// </summary>
    /// <returns>A list of <see cref="City" /> representing the cities.</returns>
    Task<List<City>> GetCities();

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>The <see cref="City" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    Task<City> GetCityById(string id);

    /// <summary>
    ///     Updates the city with the given ID in the database using the provided data.
    /// </summary>
    /// <param name="dto">The data to update the entity.</param>
    /// <param name="city">The entity that will be updated in the database.</param>
    /// <returns>A <see cref="City" /> representing the updated city.</returns>
    Task<City> UpdateCity(CityInsertDto dto, City city);
}