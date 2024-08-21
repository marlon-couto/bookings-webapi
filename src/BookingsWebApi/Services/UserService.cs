using BookingsWebApi.Context;
using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookingsWebApi.Services;

public class UserService : IUserService
{
    private readonly IBookingsDbContext _ctx;

    public UserService(IBookingsDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<UserModel> AddUser(UserInsertDto dto)
    {
        var userCreated = new UserModel
        {
            Id = Guid.NewGuid(),
            Role = "Client",
            Email = dto.Email ?? string.Empty,
            Name = dto.Name ?? string.Empty,
            Password = HashPassword.EncryptPassword(dto.Password, out var salt),
            Salt = salt,
            CreatedAt = DateTime.Now.ToUniversalTime()
        };
        await _ctx.Users.AddAsync(userCreated);
        await _ctx.SaveChangesAsync();
        return userCreated;
    }

    public async Task DeleteUser(UserModel user)
    {
        user.IsDeleted = true;
        user.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
    }

    public async Task<bool> EmailExists(string? userEmail)
    {
        var userFound = await _ctx
            .Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == userEmail && !x.IsDeleted);
        return userFound != null;
    }

    public async Task<List<UserModel>> GetUsers()
    {
        return await _ctx.Users.AsNoTracking().Where(x => !x.IsDeleted).ToListAsync();
    }

    public async Task<UserModel?> GetUserByEmail(string? userEmail)
    {
        return await _ctx.Users.FirstOrDefaultAsync(x => x.Email == userEmail && !x.IsDeleted);
    }

    public async Task<UserModel> UpdateUser(UserInsertDto dto, UserModel user)
    {
        user.Email = dto.Email ?? string.Empty;
        user.Password = HashPassword.EncryptPassword(dto.Password, out var salt);
        user.Name = dto.Name ?? string.Empty;
        user.Salt = salt;
        user.UpdatedAt = DateTime.Now.ToUniversalTime();
        await _ctx.SaveChangesAsync();
        return user;
    }

    public async Task<UserModel?> GetUserById(Guid? id)
    {
        return await _ctx.Users.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }
}
