
namespace OOP_Project_Kovba.ViewModels
{
    public class PlannedTripsViewModel
    {
       public IEnumerable<TripResultViewModel> DriverTrips { get; set; } = new List<TripResultViewModel>();
       public IEnumerable<BookingResultViewModel> PassengerBookings { get; set; } = new List<BookingResultViewModel>();
    }
}
