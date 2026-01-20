using Microsoft.EntityFrameworkCore;
using MovieTicketBookingAPI.Data;
using MovieTicketBookingAPI.Dto;
using MovieTicketBookingAPI.Models;

namespace MovieTicketBookingAPI.Services
{
    public class BookingServices:IBookingServices
    {

        private readonly AppDbContext _context;

        public BookingServices(AppDbContext context)
        {
            _context = context;
        }


        //Adding the show 
        public async Task<int> AddShowAsync(string ShowName)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {

              
                    var show = new Show
                    {
                        ShowName = ShowName,
                        Seats = new List<Seat>()
                    };

                    for (char row = 'A'; row <= 'D'; row++)
                    {
                        for (int seatNo = 1; seatNo <= 10; seatNo++)
                        {
                            show.Seats.Add(new Seat
                            {
                                SeatNumber = $"{row}{seatNo}",
                                Status = Status.Available
                            });
                        }
                    }

                    _context.Shows.Add(show);

                    
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return show.ShowId;
            }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

        }

        //Get the current status of the seats for a show

        public async Task<List<SeatStatusDto>> GetSeatStatusAsync(int showId)
        {

            return await _context.Seats
                .AsNoTracking() 
                .Where(s => s.ShowId == showId)
                .OrderBy(s => s.SeatNumber)
                .Select(s => new SeatStatusDto
                {
                    SeatNumber = s.SeatNumber,
                    Status = s.Status.ToString()
                })
                .ToListAsync();

        }

        //holding the seats
        public async Task HoldSeatsAsync(int showId, List<string> seatNumbers)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var seats = await _context.Seats
                    .Where(s => s.ShowId == showId &&
                                seatNumbers.Contains(s.SeatNumber))
                    .ToListAsync();



                var unavailableSeats = seats
                           .Where(s => s.Status != Status.Available)
                           .Select(s => new
                           {
                               s.SeatNumber,
                               s.Status
                           })
                           .ToList();

                if (unavailableSeats.Any())
                {
                    var seatInfo = string.Join(", ",
                        unavailableSeats.Select(s => $"{s.SeatNumber} ({s.Status})"));

                    throw new Exception($"These seats are not available: {seatInfo}");
                }

                foreach (var seat in seats)
                {
                    seat.Status = Status.Hold;
                    seat.HoldingTime = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> ConfirmBookingAsync(int showId, List<string> seatNumbers)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var seats = await _context.Seats
                .Where(s => s.ShowId == showId &&
                 seatNumbers.Contains(s.SeatNumber))
                .ToListAsync();

                if (seats.Count != seatNumbers.Count)
                    throw new Exception("Some seats do not exist.");

                var expiryTime = DateTime.UtcNow.AddMinutes(-5);

                if (seats.Any(s =>
                    s.Status != Status.Hold ||
                    s.HoldingTime == null ||
                    s.HoldingTime < expiryTime))
                {
                    throw new Exception("Some seats are no longer held or hold has expired.");
                }

                var booking = new Booking
                {
                    ShowId=showId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

               
                foreach (var seat in seats)
                {
                    seat.Status = Status.Booked;
                    seat.BookingId = booking.BookingId;
                    seat.HoldingTime = null;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return booking.BookingId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }



    
}
