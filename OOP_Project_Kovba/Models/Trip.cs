namespace OOP_Project_Kovba.Models
{
    public class Trip : AuditableEntity
    {
        public int Id { get; set; }
        public string FromCity { get; set; } = string.Empty;
        public string FromStreetAndHouse { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public string ToStreetAndHouse { get; set; } = string.Empty;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CarModel { get; set; } = string.Empty;
        public int MaxPassengers { get; set; }
        public decimal Price { get; set; }
        public bool IsCancelled { get; set; } = false;

        public string DriverId { get; set; } = string.Empty;
        public required ApplicationUser Driver { get; set; }
        public ICollection<ApplicationUser>? Passengers { get; set; }

        public static int MaxSeatsForTrip { get; set; } = 50;
        public static int MaxTripDurationInHours { get; set; } = 100;

        public bool HasAvailableSeats()
        {
            throw new NotImplementedException();
        }
        public void ChangePrice(decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public override string GetInfo()
        {
            throw new NotImplementedException();
        }

        public void CancelTrip()
        {
            throw new NotImplementedException();
        }

        public bool IsTripModifiable(List<ApplicationUser> passengers)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidSeats(int seats)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidTripDuration(DateTime arrivalTime, DateTime departureTime)
        {
            throw new NotImplementedException();
        }
    }
}
