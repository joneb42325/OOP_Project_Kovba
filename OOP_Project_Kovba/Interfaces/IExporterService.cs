using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Interfaces
{
    public interface IExporterService
    {
        public void ExportPlannedTripsToWord(string userId, IEnumerable<Trip> driverTrips, IEnumerable<Booking> passengerBookings);
        public void ExportPlannedTripsToExcel(string userId, IEnumerable<Trip> driverTrips, IEnumerable<Booking> passengerBookings);

        //test
        //git integration
    }
}
