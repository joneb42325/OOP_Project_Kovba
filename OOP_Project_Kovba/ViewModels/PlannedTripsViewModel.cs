
namespace OOP_Project_Kovba.ViewModels
{
    public class PlannedTripsViewModel
    {
       public IEnumerable<TripResultViewModel> DriverTrips { get; set; }
       public IEnumerable<TripResultViewModel> PassengerBookings { get; set; }
    }
}
