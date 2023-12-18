using AutoMapper;

using BookingsWebApi.DTOs;
using BookingsWebApi.Helpers;
using BookingsWebApi.Models;
using BookingsWebApi.Services;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Authorize(Policy = "Admin")]
public class RoomController : ControllerBase
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

    /// <summary>
    ///     Retrieves all rooms information from the database.
    /// </summary>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <response code="200">Returns 200 and the rooms data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        List<Room> rooms = await _service.GetRooms();
        List<RoomDto> roomsMapped = rooms.Select(r => _mapper.Map<RoomDto>(r)).ToList();

        return Ok(new ControllerResponse<List<RoomDto>> { Data = roomsMapped, Result = "Success" });
    }

    /// <summary>
    ///     Creates a new room for based on the provided data.
    /// </summary>
    /// <param name="dto">The data for creating a new room.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     POST /api/room
    ///     {
    ///     "name": "New Room",
    ///     "capacity": 1,
    ///     "image": "https://img.url",
    ///     "hotelId": "1"
    ///     }
    /// </remarks>
    /// <response code="201">Returns 201 and the newly created room data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the associated hotel is not found, returns 404 and a error message.
    /// </response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto dto)
    {
        try
        {
            await ValidateInputData(dto);

            Hotel hotelFound = await _service.GetHotelById(dto.HotelId);

            Room roomCreated = await _service.AddRoom(dto, hotelFound);
            RoomDto roomMapped = _mapper.Map<RoomDto>(roomCreated);

            return Created(
                $"/api/room/{dto.HotelId}",
                new ControllerResponse<RoomDto> { Data = roomMapped, Result = "Success" }
            );
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(
                new ControllerErrorResponse { Message = e.Message, Result = "Error" }
            );
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ControllerErrorResponse { Message = e.Message, Result = "Error" });
        }
        catch (ArgumentException e)
        {
            return BadRequest(
                new ControllerErrorResponse { Message = e.Message, Result = "Error" }
            );
        }
    }

    /// <summary>
    ///     Updates the room with the given ID based on the provided data.
    /// </summary>
    /// <param name="dto">The data for updating the room retrieved.</param>
    /// <param name="id">The ID of the room to update.</param>
    /// <returns>A JSON response representing the result of the operation.</returns>
    /// <remarks>
    ///     Sample request:
    ///     PUT /api/room/1
    ///     {
    ///     "name": "New Room v2",
    ///     "capacity": 2,
    ///     "image": "https://img.url",
    ///     "hotelId": "1"
    ///     }
    /// </remarks>
    /// <response code="200">Returns 200 and the updated room data.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="400">
    ///     If the input data is invalid, returns 400 and an error message.
    /// </response>
    /// <response code="404">
    ///     If a room with the provided ID not exists or the associated hotel is not found, returns 404 and an error
    ///     message.
    /// </response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] RoomInsertDto dto, string id)
    {
        try
        {
            await ValidateInputData(dto);

            Hotel hotelFound = await _service.GetHotelById(dto.HotelId);
            Room roomFound = await _service.GetRoomById(id);

            Room roomUpdated = await _service.UpdateRoom(dto, roomFound, hotelFound);
            RoomDto roomMapped = _mapper.Map<RoomDto>(roomUpdated);

            return Ok(new ControllerResponse<RoomDto> { Data = roomMapped, Result = "Success" });
        }
        catch (ArgumentException e)
        {
            return BadRequest(
                new ControllerErrorResponse { Message = e.Message, Result = "Error" }
            );
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ControllerErrorResponse { Message = e.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes a room with the given ID.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">
    ///     If the room is not found, returns 404 and an error message.
    /// </response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            Room roomFound = await _service.GetRoomById(id);
            await _service.DeleteRoom(roomFound);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new ControllerErrorResponse { Message = e.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(RoomInsertDto dto)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            List<string> errorMessages = validationResult
                .Errors
                .Select(e => e.ErrorMessage)
                .ToList();
            throw new ArgumentException(string.Join(" ", errorMessages));
        }
    }
}