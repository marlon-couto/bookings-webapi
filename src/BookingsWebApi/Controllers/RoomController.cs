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
public class RoomController : ControllerBase, IRoomController
{
    private readonly IMapper _mapper;
    private readonly IRoomService _service;
    private readonly IValidator<RoomInsertDto> _validator;

    public RoomController(IRoomService service, IMapper mapper, IValidator<RoomInsertDto> validator)
    {
        _service = service;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var rooms = await _service.GetRooms();
        var roomsMapped = rooms.Select(r => _mapper.Map<RoomDto>(r));
        return Ok(new ControllerResponse { Data = roomsMapped });
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto dto)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var hotelFound = await _service.GetHotelById(dto.HotelId);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the provided id does not exist.");
        }

        var roomCreated = await _service.AddRoom(dto, hotelFound);
        var roomMapped = _mapper.Map<RoomDto>(roomCreated);
        return Created($"/api/room/{dto.HotelId}", new ControllerResponse { Data = roomMapped, StatusCode = 201 });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync([FromBody] RoomInsertDto dto, Guid id)
    {
        var errors = await GetInputDataErrors(dto);
        if (errors != null)
        {
            throw new InvalidInputDataException(string.Join(" ", errors));
        }

        var hotelFound = await _service.GetHotelById(dto.HotelId);
        if (hotelFound == null)
        {
            throw new NotFoundException("The hotel with the provided id does not exist.");
        }

        var roomFound = await _service.GetRoomById(id);
        if (roomFound == null)
        {
            throw new NotFoundException("The room with the provided id does not exist.");
        }

        var roomUpdated = await _service.UpdateRoom(dto, roomFound, hotelFound);
        var roomMapped = _mapper.Map<RoomDto>(roomUpdated);
        return Ok(new ControllerResponse { Data = roomMapped });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var roomFound = await _service.GetRoomById(id);
        if (roomFound == null)
        {
            throw new NotFoundException("The room with the provided id does not exist.");
        }

        await _service.DeleteRoom(roomFound);
        return NoContent();
    }

    private async Task<IEnumerable<string>?> GetInputDataErrors(RoomInsertDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (validationResult.IsValid)
        {
            return null;
        }

        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
        return errorMessages;
    }
}