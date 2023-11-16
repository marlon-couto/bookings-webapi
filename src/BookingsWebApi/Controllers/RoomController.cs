using BookingsWebApi.Dtos;
using BookingsWebApi.Models;
using BookingsWebApi.Repositories;

using FluentValidation;

using Microsoft.AspNetCore.Mvc;

namespace BookingsWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _repository;
        private readonly IValidator<RoomInsertDto> _validator;
        public RoomController(IRoomRepository repository, IValidator<RoomInsertDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            try
            {
                Room roomFound = await _repository.GetRoomById(id);
                _repository.DeleteRoom(roomFound);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetAsync(string hotelId)
        {
            try
            {
                await _repository.GetHotelById(hotelId);

                List<RoomDto> hotelRooms = await _repository.GetHotelRooms(hotelId);
                return Ok(hotelRooms);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto inputData)
        {
            try
            {
                await ValidateInputData(inputData);

                Hotel hotelFound = await _repository.GetHotelById(inputData.HotelId);

                RoomDto createdRoom = await _repository.AddRoom(inputData, hotelFound);
                return Created($"/api/room/{inputData.HotelId}", createdRoom);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        private async Task ValidateInputData(RoomInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException(validationResult.Errors[0].ErrorMessage);
            }
        }
    }
}
