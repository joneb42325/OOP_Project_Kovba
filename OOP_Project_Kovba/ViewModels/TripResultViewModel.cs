namespace OOP_Project_Kovba.ViewModels
{
    public class TripResultViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int MaxPassengers { get; set; }
        public decimal Price { get; set; }
    }
}
