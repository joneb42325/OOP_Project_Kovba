namespace OOP_Project_Kovba.Services
{
    public class TripBookedEventArgs : EventArgs
    {
       public string TripId { get; set; } = string.Empty;
       public string UserId { get; set; } = string.Empty;
       public int SeatsBooked { get; set; }
    }
}
