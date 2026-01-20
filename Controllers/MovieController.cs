using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingAPI.Dto;
using MovieTicketBookingAPI.Models;
using MovieTicketBookingAPI.Services;

namespace MovieTicketBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public MovieController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpPost("addshow")]

        public async Task<IActionResult> CreateShow([FromQuery] string showName)
        {
            if (string.IsNullOrWhiteSpace(showName))
            {
                return BadRequest("Show name is required.");

            }


            try
            {
                    var showId = await _seatService.AddShowAsync(showName);

                    return Ok(new
                    {
                        Success = true,
                        Message="Show created Successfully",
                        Data= showId

                    });
            }
                
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        Sucess=false,
                        Message = "An error occurred while creating the show",
                        Error = ex.Message
                    });

                }
        }

        [HttpGet("seats")]
        public async Task<IActionResult> GetSeatStatus(int showId)
        {
            var seats= await _seatService.GetSeatStatusAsync(showId);

            if(!seats.Any())
            {
                return NotFound( new
                {
                    Sucess = false,
                    Message = "Show Not Found"
                });
            }

            return Ok(new
            {
                Sucess = true,
                Message = "Seat status retrieved successfully",
                Data = seats
            });

        }

        [HttpPost("hold")]

        public async Task<IActionResult> HoldSeats([FromBody] BookingRequestDto request)
        {

            if (request == null || !request.SeatNumbers.Any())
                return BadRequest("ShowId and SeatNumbers are required.");

            try
            {
                await _seatService.HoldSeatsAsync(
                    request.ShowId,
                    request.SeatNumbers
                    
                );

                return Ok(new
                {
                    Success = true,
                    Message = "Seats held successfully",
                    ExpiresInMinutes = 2
                });
            }
            catch (Exception ex)
            {

                return Conflict( new
                {
                    Success = false,
                    Message = ex.Message,
                   
                });
            }

        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmBooking([FromBody] BookingRequestDto request)
        {

            if (request == null || !request.SeatNumbers.Any())
                return BadRequest("ShowId and SeatNumbers are required.");

            try
            {
                var bookingId = await _seatService.ConfirmBookingAsync(
                    request.ShowId,
                    request.SeatNumbers
                );

                return Ok(new
                {
                    Success = true,
                    Message = "Booking confirmed successfully",
                    Data = bookingId
                });
            }
            catch (Exception ex)
            {
                return Conflict(new
                {
                    Success = false,
                    Message = ex.Message,

                });

            }

        }





    }
}
