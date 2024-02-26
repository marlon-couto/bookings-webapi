using System;
using System.Collections.Generic;
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
        UserInsertDto dto =
            new()
            {
                Email = _faker.Internet.Email(),
                Name = _faker.Name.FirstName(),
                Password = _faker.Internet.Password()
            };
        User userCreated = await _service.AddUser(dto);

        userCreated.Should().NotBeNull();
    }

    [Fact(DisplayName = "DeleteUser should remove user")]
    public async Task DeleteUser_ShouldRemoveUser()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _service.DeleteUser(user);

        List<User> users = await _context.Users.AsNoTracking().ToListAsync();
        users.Count.Should().Be(0);
    }

    [Fact(DisplayName = "EmailExists not throw if email not exists")]
    public async Task EmailExists_NotThrow_IfEmailNotExists()
    {
        Func<Task> act = async () => await _service.EmailExists(_faker.Internet.Email());

        await act.Should().NotThrowAsync();
    }

    [Fact(DisplayName = "EmailExists throw InvalidOperationException if email exists")]
    public async Task EmailExists_ThrowInvalidOperationException_IfEmailExists()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        Func<Task> act = async () => await _service.EmailExists(user.Email);

        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("The email provided is already registered.");
    }

    [Fact(DisplayName = "GetUsers should return all users")]
    public async Task GetUsers_ShouldReturnAllUsers()
    {
        User user1 = UserBuilder.New().Build();
        User user2 = UserBuilder.New().Build();
        await _context.Users.AddAsync(user1);
        await _context.Users.AddAsync(user2);
        await _context.SaveChangesAsync();

        List<User> users = await _service.GetUsers();

        users.Count.Should().Be(2);
    }

    [Fact(DisplayName = "GetUserByEmail should return user found")]
    public async Task GetUserByEmail_ShouldReturnUserFound()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        User userFound = await _service.GetUserByEmail(user.Email);

        userFound.Should().NotBeNull();
    }

    [Fact(DisplayName = "GetUserByEmail throw UnauthorizedAccessException if email not exists")]
    public async Task GetUserByEmail_ThrowUnauthorizedAccessException_IfEmailNotExists()
    {
        Func<Task<User>> act = async () => await _service.GetUserByEmail(_faker.Internet.Email());

        await act.Should()
            .ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "UpdateUser should update user")]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        UserInsertDto dto =
            new()
            {
                Email = _faker.Internet.Email(),
                Password = _faker.Internet.Password(),
                Name = _faker.Name.FirstName()
            };
        User userUpdated = await _service.UpdateUser(dto, user);

        userUpdated.Should().NotBeNull();
    }
}
