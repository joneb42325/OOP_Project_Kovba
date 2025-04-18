namespace OOP_Project_Kovba.Models
{
    public class Booking : AuditableEntity
    {
        public int Id { get; set; }
        public int SeatsBooked { get; set; }

        public string UserId { get; set; } = string.Empty;
        public required ApplicationUser User { get; set; }

        public string TripId { get; set; } = string.Empty;
        public required Trip Trip { get; set; }

        public void ChangeSeats(int newSeats)
        {
            throw new NotImplementedException();
        }

        public override string GetInfo()
        {
            throw new NotImplementedException();
        }

        public void CancelBooking()
        {
            throw new NotImplementedException();
        }

        public static bool IsUserAlreadyBooked(string userId, string tripId, ICollection<Booking> bookings)
        {
            throw new NotImplementedException();
        }
    }
}
