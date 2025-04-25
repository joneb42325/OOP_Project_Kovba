namespace OOP_Project_Kovba.ViewModels
{
    public class SearchTripViewModel
    {
        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int Passengers { get; set; }
    }
}
