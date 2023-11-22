using AutoMapper;

using BookingsWebApi.DTOs;
using BookingsWebApi.Models;

namespace BookingsWebApi;

/// <inheritdoc />
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<City, CityDto>();

        CreateMap<Hotel, HotelDto>()
            .ForMember(dest => dest.CityName,
                opts => opts.MapFrom(src => src.City!.Name))
            .ForMember(dest => dest.CityState,
                opts => opts.MapFrom(src => src.City!.State));

        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Hotel, opts => CreateMap<Hotel, HotelDto>());

        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.Room, opts => CreateMap<Room, RoomDto>());
    }
}