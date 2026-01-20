using MovieTicketBookingAPI.Dto;

namespace MovieTicketBookingAPI.Services
{
    public interface IBookingServices
    {
        Task<int> AddShowAsync(string ShowName);
        Task<List<SeatStatusDto>> GetSeatStatusAsync(int showId);
        Task HoldSeatsAsync(int showId, List<string> seatNumbers);
        Task<int> ConfirmBookingAsync(int showId, List<string> seatNumbers);
    }
}
