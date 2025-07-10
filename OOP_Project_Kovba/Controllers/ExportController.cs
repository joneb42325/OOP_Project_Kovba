using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Controllers
{
    public class ExportController : BaseController
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ITripService _tripService;
        private readonly IExporterService _exporterService;

        public ExportController(ITripRepository tripRepository, IBookingRepository bookingRepository, UserManager<ApplicationUser> userManager, ITripService tripService, IExporterService exporterService) : base(userManager)
        {
            _tripRepository = tripRepository;
            _bookingRepository = bookingRepository;
            _tripService = tripService;
            _exporterService = exporterService;
        }

        public async Task<IActionResult> ExportPlannedTripsToWord()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Login", "Account");
            }
            var driverTrips = await _tripRepository.GetAllDriverTrips(userId);
            var passengerBookings = await _bookingRepository.GetAllUsersBookings(userId);

            _exporterService.ExportPlannedTripsToWord(userId, driverTrips, passengerBookings);

            TempData["TripMesage"] = "Дані експортовано.";
            return RedirectToAction("PlannedTrips");
        }

        public async Task<IActionResult> ExportPlannedTripsToExcel()
        {
            var userId = _userManager.GetUserId(User);
            if (userId is null)
            {
                return RedirectToAction("Login", "Account");
            }

            var driverTrips = await _tripRepository.GetAllDriverTrips(userId);
            var passengerBookings = await _bookingRepository.GetAllUsersBookings(userId);


            _exporterService.ExportPlannedTripsToExcel(userId, driverTrips, passengerBookings);

            TempData["TripMesage"] = "Дані експортовано.";
            return RedirectToAction("PlannedTrips");
        }
    }
}
