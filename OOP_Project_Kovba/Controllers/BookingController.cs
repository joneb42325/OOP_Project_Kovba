using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OOP_Project_Kovba.Interfaces;
using OOP_Project_Kovba.Data;
using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Controllers
{
    public class BookingController : BaseController
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ITripRepository _tripRepository;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripService _tripService;

        public BookingController(IBookingRepository bookingRepository, ITripRepository tripRepository, UserManager<ApplicationUser> userManager, ITripService tripService) : base(userManager)
        {
            _bookingRepository = bookingRepository;
            _tripRepository = tripRepository;
           // _userManager = userManager;
            _tripService = tripService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateBooking(string tripId, int seatsBooked)
        {
                var userId = _userManager.GetUserId(User);
                if (userId == null)
                    return Unauthorized();

                var result = await _tripService.TryCreateBookingAsync(tripId, userId, seatsBooked);

                TempData["SuccessMessage"] = result.Message;
                return View("~/Views/Trip/TripDetails.cshtml", result.ViewModel);
            }
        }
    }