namespace MovieTicketBookingAPI.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int ShowId { get; set; }


        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public ICollection<Seat> Seats { get; set; }


    }



}
