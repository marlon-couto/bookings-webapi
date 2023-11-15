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

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetAsync(string hotelId)
        {
            Hotel? hotelFound = await _repository.GetHotelById(hotelId);
            if (hotelFound is null)
            {
                return NotFound(new { Message = "The hotel with the provided id does not exist" });
            }

            List<RoomDto> hotelRooms = await _repository.GetHotelRooms(hotelId);
            return Ok(hotelRooms);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] RoomInsertDto inputData)
        {
            var validationResult = await _validator.ValidateAsync(inputData);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Message = validationResult.Errors[0].ErrorMessage });
            }

            Hotel? hotelFound = await _repository.GetHotelById(inputData.HotelId);
            if (hotelFound is null)
            {
                return NotFound(new { Message = "The hotel with the provided id does not exist" });
            }

            RoomDto createdRoom = await _repository.AddRoom(inputData, hotelFound);
            return Created($"/api/room/{inputData.HotelId}", createdRoom);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            Room? roomFound = await _repository.GetRoomById(id);
            if (roomFound is null)
            {
                return NotFound(new { Message = "The room with the provided id does not exist" });
            }

            _repository.DeleteRoom(roomFound);
            return NoContent();
        }
    }
}
