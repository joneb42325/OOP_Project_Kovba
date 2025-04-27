using OOP_Project_Kovba.ViewModels;

namespace OOP_Project_Kovba.Interfaces
{
    public interface ITripService
    {
        Task<TripDetailsViewModel?> GetTripDetailsViewModelAsync(string id);
    }
}
