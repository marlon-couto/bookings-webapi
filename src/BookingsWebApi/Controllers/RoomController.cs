using AutoMapper;
using BookingsWebApi.DTOs;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;
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
    private readonly IRoomRepository _roomRepository;
    private readonly IValidator<RoomInsertDto> _validator;

    public RoomController(
        IRoomRepository roomRepository,
        IMapper mapper,
        IValidator<RoomInsertDto> validator
    )
    {
        _roomRepository = roomRepository;
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
        List<Room> allRooms = await _roomRepository.GetAllRooms();
        return Ok(new { Data = allRooms.Select(r => _mapper.Map<RoomDto>(r)), Result = "Success" });
    }

    /// <summary>
    ///     Creates a new room for based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for creating a new room.</param>
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
    /// <response code="404">If the associated hotel is not found, returns 404 and a error message.</response>
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto inputData)
    {
        try
        {
            await ValidateInputData(inputData);

            Hotel hotelFound = await _roomRepository.GetHotelById(inputData.HotelId);

            Room createdRoom = await _roomRepository.AddRoom(inputData, hotelFound);
            return Created(
                $"/api/room/{inputData.HotelId}",
                new { Data = _mapper.Map<RoomDto>(createdRoom), Result = "Success" }
            );
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Updates the room with the given ID based on the provided data.
    /// </summary>
    /// <param name="inputData">The data for updating the room retrieved.</param>
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
    /// <response code="400">If the input data is invalid, returns 400 and an error message.</response>
    /// <response code="404">
    ///     If a room with the provided ID not exists or the associated hotel is not found, returns 404 and
    ///     an error message.
    /// </response>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromBody] RoomInsertDto inputData, string id)
    {
        try
        {
            await ValidateInputData(inputData);

            Hotel hotelFound = await _roomRepository.GetHotelById(inputData.HotelId);
            Room roomFound = await _roomRepository.GetRoomById(id);

            Room updatedRoom = await _roomRepository.UpdateRoom(inputData, roomFound, hotelFound);
            return Ok(new { Data = _mapper.Map<RoomDto>(updatedRoom), Result = "Success" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message, Result = "Error" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    /// <summary>
    ///     Deletes a room with the given ID.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <returns>A status code 204 and no content.</returns>
    /// <response code="204">Returns 204 with no content.</response>
    /// <response code="401">If the user is unauthorized, returns 401.</response>
    /// <response code="404">If the room is not found, returns 404 and an error message.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        try
        {
            Room roomFound = await _roomRepository.GetRoomById(id);
            await _roomRepository.DeleteRoom(roomFound);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { ex.Message, Result = "Error" });
        }
    }

    private async Task ValidateInputData(RoomInsertDto inputData)
    {
        ValidationResult? validationResult = await _validator.ValidateAsync(inputData);
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
