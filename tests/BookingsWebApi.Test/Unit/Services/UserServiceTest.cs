using System;
using System.Threading.Tasks;
using Bogus;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Helpers;
using BookingsWebApi.Test.Helpers.Builders;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BookingsWebApi.Test.Unit.Services;

public class UserServiceTest : IClassFixture<TestFixture>, IDisposable
{
    private readonly TestDbContext _context;
    private readonly Faker _faker = new();
    private readonly UserService _service;

    public UserServiceTest(TestFixture fixture)
    {
        _context = fixture.Context;
        _service = new UserService(_context);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        GC.SuppressFinalize(this);
    }

    [Fact(DisplayName = "AddUser should add user")]
    public async Task AddUser_ShouldAddUser()
    {
        var dto = new UserInsertDto
        {
            Email = _faker.Internet.Email(),
            Name = _faker.Name.FirstName(),
            Password = _faker.Internet.Password()
        };
        var userCreated = await _service.AddUser(dto);
        userCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteUser should remove user")]
    public async Task DeleteUser_ShouldRemoveUser()
    {
        var user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        await _service.DeleteUser(user);
        var users = await _context.Users.AsNoTracking().ToListAsync();
        users.Count.Should().Be(0);
    }

    [Fact(DisplayName = "EmailExists not throw if email not exists")]
    public async Task EmailExists_NotThrow_IfEmailNotExists()
    {
        var act = async () => await _service.EmailExists(_faker.Internet.Email());
        await act.Should().NotThrowAsync();
    }

    [Fact(DisplayName = "EmailExists return true if email exists")]
    public async Task EmailExists_ReturnTrue_IfEmailExists()
    {
        var user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        var emailExists =  await _service.EmailExists(user.Email);
        emailExists.Should().BeTrue();
    }

    [Fact(DisplayName = "GetUsers should return all users")]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        var user1 = UserBuilder.New().Build();
        var user2 = UserBuilder.New().Build();
        await _context.Users.AddAsync(user1);
        await _context.Users.AddAsync(user2);
        await _context.SaveChangesAsync();
        var users = await _service.GetUsers();
        users.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetUserByEmail should return user found")]
    public async Task GetUserByEmail_ShouldReturnUserFound()
    {
        var user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        var userFound = await _service.GetUserByEmail(user.Email);
        userFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetUserByEmail return null if email not exists")]
    public async Task GetUserByEmail_ReturnNull_IfEmailNotExists()
    {
        var userFound = await _service.GetUserByEmail(_faker.Internet.Email());
        userFound.Should().BeNull();
    }

    [Fact(DisplayName = "UpdateUser should update user")]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        var user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        var dto = new UserInsertDto
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(),
            Name = _faker.Name.FirstName()
        };
        var userUpdated = await _service.UpdateUser(dto, user);
        userUpdated.Should().NotBeNull();
    }
}