using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Services.Interfaces;

/// <summary>
///     Service for managing users in the application.
/// </summary>
public interface IUserService
{
    /// <summary>
    ///     Adds a new user to the database.
    /// </summary>
    /// <param name="dto">The data to create a new user.</param>
    /// <returns>A <see cref="UserModel" /> representing the newly created user.</returns>
    Task<UserModel> AddUser(UserInsertDto dto);

    /// <summary>
    ///     Deletes a user with the given ID from the database.
    /// </summary>
    /// <param name="user">The entity that will be deleted.</param>
    Task DeleteUser(UserModel user);

    /// <summary>
    ///     Checks if the given email already exists in the database.
    /// </summary>
    /// <param name="userEmail">The email that will be verifies.</param>
    Task<bool> EmailExists(string? userEmail);

    /// <summary>
    ///     Retrieves all users from the database.
    /// </summary>
    /// <returns>A list of <see cref="UserModel" /> representing the users.</returns>
    Task<List<UserModel>> GetUsers();

    /// <summary>
    ///     Retrieves a user in the database with the given email.
    /// </summary>
    /// <param name="userEmail">The email associated with the user.</param>
    /// <returns>A <see cref="UserModel" /> representing the user data.</returns>
    Task<UserModel?> GetUserByEmail(string? userEmail);

    /// <summary>
    ///     Updates a user in the database using the provided data.
    /// </summary>
    /// <param name="dto">The data to update the user.</param>
    /// <param name="user">The entity that will be updated.</param>
    /// <returns>A <see cref="UserModel" /> representing the updated user.</returns>
    Task<UserModel> UpdateUser(UserInsertDto dto, UserModel user);
}