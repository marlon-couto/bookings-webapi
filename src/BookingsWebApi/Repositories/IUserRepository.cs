using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Repositories;

/// <summary>
///     Repository for managing users in the database.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    ///     Adds a new user to the database.
    /// </summary>
    /// <param name="inputData">The data to create a new user.</param>
    /// <returns>A <see cref="User" /> representing the newly created user.</returns>
    Task<User> AddUser(UserInsertDto inputData);

    /// <summary>
    ///     Deletes a user with the given ID from the database.
    /// </summary>
    /// <param name="user">The entity that will be deleted.</param>
    Task DeleteUser(User user);

    /// <summary>
    ///     Checks if the given email already exists in the database.
    /// </summary>
    /// <param name="userEmail">The email that will be verifies.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the email already exists in the database.
    /// </exception>
    Task EmailExists(string userEmail);

    /// <summary>
    ///     Retrieves all users from the database.
    /// </summary>
    /// <returns>A list of <see cref="User" /> representing the users.</returns>
    Task<List<User>> GetAllUsers();

    /// <summary>
    ///     Retrieves a user in the database with the given email.
    /// </summary>
    /// <param name="userEmail">The email associated with the user.</param>
    /// <returns>A <see cref="User" /> representing the user data.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if a user with the given email does not exist in the database.
    /// </exception>
    Task<User> GetUserByEmail(string userEmail);

    /// <summary>
    ///     Updates a user in the database using the provided data.
    /// </summary>
    /// <param name="inputData">The data to update the user.</param>
    /// <param name="user">The entity that will be updated.</param>
    /// <returns>A <see cref="User" /> representing the updated user.</returns>
    Task<User> UpdateUser(UserInsertDto inputData, User user);
}
