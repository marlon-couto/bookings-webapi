using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

/// <summary>
///     Service for managing users in the application.
/// </summary>
public class UserService
{
    private readonly IBookingsDbContext _context;

    public UserService(IBookingsDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Adds a new user to the database.
    /// </summary>
    /// <param name="dto">The data to create a new user.</param>
    /// <returns>A <see cref="User" /> representing the newly created user.</returns>
    public async Task<User> AddUser(UserInsertDto dto)
    {
        User user =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Role = "Client",
                Email = dto.Email,
                Name = dto.Name,
                Password = dto.Password
            };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }

    /// <summary>
    ///     Deletes a user with the given ID from the database.
    /// </summary>
    /// <param name="user">The entity that will be deleted.</param>
    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    ///     Checks if the given email already exists in the database.
    /// </summary>
    /// <param name="userEmail">The email that will be verifies.</param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the email already exists in the database.
    /// </exception>
    public async Task EmailExists(string userEmail)
    {
        User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (userFound != null)
        {
            throw new InvalidOperationException("The email provided is already registered.");
        }
    }

    /// <summary>
    ///     Retrieves all users from the database.
    /// </summary>
    /// <returns>A list of <see cref="User" /> representing the users.</returns>
    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    /// <summary>
    ///     Retrieves a user in the database with the given email.
    /// </summary>
    /// <param name="userEmail">The email associated with the user.</param>
    /// <returns>A <see cref="User" /> representing the user data.</returns>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown if a user with the given email does not exist in the database.
    /// </exception>
    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail)
               ?? throw new UnauthorizedAccessException("The email or password provided is incorrect.");
    }

    /// <summary>
    ///     Updates a user in the database using the provided data.
    /// </summary>
    /// <param name="dto">The data to update the user.</param>
    /// <param name="user">The entity that will be updated.</param>
    /// <returns>A <see cref="User" /> representing the updated user.</returns>
    public async Task<User> UpdateUser(UserInsertDto dto, User user)
    {
        user.Email = dto.Email;
        user.Password = dto.Password;
        user.Name = dto.Name;
        await _context.SaveChangesAsync();

        return user;
    }
}