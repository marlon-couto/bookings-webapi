using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories;

/// <summary>
///     Repository for managing cities in the application.
/// </summary>
public interface ICityRepository
{
    /// <summary>
    ///     Adds a new city to the database.
    /// </summary>
    /// <param name="inputData">The data to create a new city.</param>
    /// <returns>A <see cref="City" /> representing the newly created city.</returns>
    public Task<City> AddCity(CityInsertDto inputData);

    /// <summary>
    ///     Deletes a city with the given ID from the database.
    /// </summary>
    /// <param name="city">The entity that will be removed from the database.</param>
    public Task DeleteCity(City city);

    /// <summary>
    ///     Retrieves all cities from the database.
    /// </summary>
    /// <returns>A list of <see cref="City" /> representing the cities.</returns>
    public Task<List<City>> GetAllCities();

    /// <summary>
    ///     Retrieves a city with the given ID from the database.
    /// </summary>
    /// <param name="id">The city ID to search the database.</param>
    /// <returns>The <see cref="City" /> found.</returns>
    /// <exception cref="KeyNotFoundException">
    ///     Thrown if a city with the given ID does not exist.
    /// </exception>
    public Task<City> GetCityById(string id);

    /// <summary>
    ///     Updates the city with the given ID in the database using the provided data.
    /// </summary>
    /// <param name="inputData">The data to update the entity.</param>
    /// <param name="city">The entity that will be updated in the database.</param>
    /// <returns>A <see cref="City" /> representing the updated city.</returns>
    public Task<City> UpdateCity(CityInsertDto inputData, City city);
}