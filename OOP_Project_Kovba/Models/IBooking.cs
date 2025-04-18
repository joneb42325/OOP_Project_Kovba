namespace OOP_Project_Kovba.Models
{
    public interface IBooking
    {
        void ChangeSeats(int newSeats);
        string GetDetails();
        void CancelBooking();
        bool IsUserAlreadyBooked(string userId, string tripId, ICollection<Booking> bookings);
    }
}
