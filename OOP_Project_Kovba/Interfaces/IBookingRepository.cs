using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Interfaces
{
    public interface IBookingRepository
    {
        public Task AddBookingAsync(Booking booking);
        public Task<IEnumerable<Booking>> GetAllUsersBookings(string userId);
    }
}
