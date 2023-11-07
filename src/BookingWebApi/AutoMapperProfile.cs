using AutoMapper;

using BookingWebApi.Dtos;
using BookingWebApi.Models;

namespace BookingWebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.User, opts => opts.MapFrom(src => src.User!.Name))
                .ForMember(dest => dest.Room, opts => opts.MapFrom(src => src.Room!.Name));

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Bookings, opts => opts.MapFrom(src => src.Bookings!.ToList()
                    .Select(booking => booking.BookingId)));
        }
    }
}
