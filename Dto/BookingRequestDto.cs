namespace MovieTicketBookingAPI.Dto
{
    public class BookingRequestDto
    {
        public int ShowId { get; set; }

        public List<string> SeatNumbers { get; set; } = new();
    }
}
