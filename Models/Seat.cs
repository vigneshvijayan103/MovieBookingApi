namespace MovieTicketBookingAPI.Models
{
    public class Seat
    {
        public int SeatId { get; set; }

        public string SeatNumber { get; set; }

        public int ShowId { get; set; }

        public Status Status { get; set; }

        public DateTime? HoldingTime { get; set; }

        public int? BookingId { get; set; }


        public byte[] RowVersion { get; set; }

        public Booking Booking { get; set; }

    }
}
