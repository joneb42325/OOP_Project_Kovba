using OOP_Project_Kovba.Models;
namespace OOP_Project_Kovba.ViewModels
{
    public class TripDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public string FromStreetAndHouse { get; set; } = string.Empty;
        public string ToStreetAndHouse { get; set; } = string.Empty;
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string DriverEmail { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int MaxPassengers { get; set; }
        public string? Comment { get; set; }
        public decimal Price { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
