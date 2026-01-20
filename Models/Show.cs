namespace MovieTicketBookingAPI.Models
{
    public class Show
    {
        public int ShowId { get; set; }

        public string ShowName { get; set; }

        public ICollection<Seat> Seats { get; set; }
    }
}
