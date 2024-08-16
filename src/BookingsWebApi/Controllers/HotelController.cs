using AutoMapper;
using BookingsWebApi.Controllers.Interfaces;
using BookingsWebApi.DTOs;
using BookingsWebApi.Exceptions;
using BookingsWebApi.Helpers;
using BookingsWebApi.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class HotelController : Controller, IHotelController
{
    private readonly IMapper _mapper;
    private readonly IHotelService _service;
    private readonly IValidator<HotelInsertDto> _validator;

    public HotelController(
        IHotelService service,
        IMapper mapper,
        IValidator<HotelInsertDto> validator
    )
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> GetAsync()
    {
        var hotels = await _service.GetHotels();
        var hotelsMapped = hotels.Select(x => _mapper.Map<HotelDto>(x));
        return Ok(new ControllerResponse { Data = hotelsMapped });
    }

    [HttpGet("{id:guid}/rooms")]
    [Authorize(Policy = "Client")]
    public async Task<IActionResult> GetHotelRoomsAsync(Guid id)
    {
        var hotelFound = await _service.GetHotelById(id);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the id provided does not exist.");
        }

        var hotelRooms = await _service.GetHotelRooms(hotelFound);
        var roomsMapped = hotelRooms.Select(x => _mapper.Map<RoomDto>(x));
        return Ok(new ControllerResponse { Data = roomsMapped });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] HotelInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var cityFound = await _service.GetCityById(dto.CityId);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        var hotelCreated = await _service.AddHotel(dto, cityFound);
        var hotelMapped = _mapper.Map<HotelDto>(hotelCreated);
        return Created(
            "/api/hotel",
            new ControllerResponse { Data = hotelMapped, StatusCode = 201 }
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync([FromBody] HotelInsertDto dto, Guid id)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var hotelFound = await _service.GetHotelById(id);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the id provided does not exist.");
        }

        var cityFound = await _service.GetCityById(dto.CityId);
        if (cityFound == null)
        {
            throw new NotFoundException("The city with the id provided does not exist.");
        }

        var hotelUpdated = await _service.UpdateHotel(dto, hotelFound, cityFound);
        var hotelMapped = _mapper.Map<HotelDto>(hotelUpdated);
        return Ok(new ControllerResponse { Data = hotelMapped });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var hotelFound = await _service.GetHotelById(id);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the id provided does not exist.");
        }

        await _service.DeleteHotel(hotelFound);
        return NoContent();
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(HotelInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
        return errorMessages;
    }
}