using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi.Test.Helpers;

public static class Extensions
{
    public static UserDto AsDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Role = user.Role
        };
    }
}
