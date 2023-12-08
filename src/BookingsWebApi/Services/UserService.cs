using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class UserService : IUserService
{
    private readonly IBookingsDbContext _context;

    public UserService(IBookingsDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddUser(UserInsertDto dto)
    {
        User userCreated =
            new()
            {
                Id = Guid.NewGuid().ToString(),
                Role = "Client",
                Email = dto.Email,
                Name = dto.Name,
                Password = HashPassword.EncryptPassword(dto.Password, out string salt),
                Salt = salt
            };

        await _context.Users.AddAsync(userCreated);
        await _context.SaveChangesAsync();

        return userCreated;
    }

    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task EmailExists(string userEmail)
    {
        User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (userFound != null)
        {
            throw new InvalidOperationException("The email provided is already registered.");
        }
    }

    public async Task<List<User>> GetUsers()
    {
        List<User> users = await _context.Users.AsNoTracking().ToListAsync();
        return users;
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        User? userFound = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        return userFound
               ?? throw new UnauthorizedAccessException(
                   "The email or password provided is incorrect."
               );
    }

    public async Task<User> UpdateUser(UserInsertDto dto, User user)
    {
        user.Email = dto.Email;
        user.Password = HashPassword.EncryptPassword(dto.Password, out string salt);
        user.Name = dto.Name;
        user.Salt = salt;
        await _context.SaveChangesAsync();

        return user;
    }
}