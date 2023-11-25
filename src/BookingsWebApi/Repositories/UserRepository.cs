using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IBookingsDbContext _context;

    public UserRepository(IBookingsDbContext context)
    {
        _context = context;
    }

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

    public async Task DeleteUser(User user)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task EmailExists(string userEmail)
    {
        User? userFound =
            await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (userFound != null)
        {
            throw new InvalidOperationException(
                "The email provided is already registered");
        }
    }

    public async Task<List<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByEmail(string userEmail)
    {
        return await _context.Users.FirstOrDefaultAsync(u =>
                   u.Email == userEmail)
               ?? throw new UnauthorizedAccessException(
                   "The email or password provided is incorrect");
    }

    public async Task<User> UpdateUser(UserInsertDto dto, User user)
    {
        user.Email = dto.Email;
        user.Password = dto.Password;
        user.Name = dto.Name;
        await _context.SaveChangesAsync();

        return user;
    }
}