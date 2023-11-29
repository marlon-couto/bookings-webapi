using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Bogus;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Services;
using BookingsWebApi.Test.Builders;
using BookingsWebApi.Test.Context;
using BookingsWebApi.Test.Helpers;

using FluentAssertions;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace BookingsWebApi.Test.Services;

public class UserServiceTest
{
    private readonly TestBookingsDbContext _context;
    private readonly Faker _faker = new();
    private readonly UserService _service;

    public UserServiceTest()
    {
        _context = TestUtils.CreateContext();
        _service = new UserService(_context);
    }

    [Fact(DisplayName = "AddUser should add user")]
    public async Task AddUser_ShouldAddUser()
    {
        UserInsertDto dto = new()
        {
            Email = _faker.Internet.Email(), Name = _faker.Name.FirstName(), Password = _faker.Internet.Password()
        };
        User createdUser = await _service.AddUser(dto);

        createdUser.Should().NotBeNull();
        await _context.ClearDatabase(_context.Users);
    }

    [Fact(DisplayName = "DeleteUser should remove user")]
    public async Task DeleteUser_ShouldRemoveUser()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _service.DeleteUser(user);

        List<User> allUsers = await _context.Users.ToListAsync();
        allUsers.Count.Should().Be(0);
        await _context.ClearDatabase(_context.Users);
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

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("The email provided is already registered.");
        await _context.ClearDatabase(_context.Users);
    }

    [Fact(DisplayName = "GetAllUsers should return all users")]
    public async Task GetAllUsers_ShouldReturnAllUsers()
    {
        User user1 = UserBuilder.New().Build();
        await _context.Users.AddAsync(user1);
        User user2 = UserBuilder.New().Build();
        await _context.Users.AddAsync(user2);
        await _context.SaveChangesAsync();

        List<User> allUsers = await _service.GetAllUsers();

        allUsers.Count.Should().Be(2);
        await _context.ClearDatabase(_context.Users);
    }

    [Fact(DisplayName = "GetUserByEmail should return user")]
    public async Task GetUserByEmail_ShouldReturnUser()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        User userFound = await _service.GetUserByEmail(user.Email);

        userFound.Should().NotBeNull();
        await _context.ClearDatabase(_context.Users);
    }

    [Fact(DisplayName = "GetUserByEmail throw UnauthorizedAccessException if email not exists")]
    public async Task GetUserByEmail_ThrowUnauthorizedAccessException_IfEmailNotExists()
    {
        string userEmail = _faker.Internet.Email();
        Func<Task<User>> act = async () => await _service.GetUserByEmail(userEmail);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("The email or password provided is incorrect.");
    }

    [Fact(DisplayName = "UpdateUser should update user")]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        User user = UserBuilder.New().Build();
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        UserInsertDto dto = new()
        {
            Email = _faker.Internet.Email(), Password = _faker.Internet.Password(), Name = _faker.Name.FirstName()
        };
        User createdUser = await _service.UpdateUser(dto, user);

        createdUser.Should().NotBeNull();
        await _context.ClearDatabase(_context.Users);
    }
}