using Bookify.BL.Interfaces;
using Bookify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Owner")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public async Task<IActionResult> Index()
        {
            string? ownerId = null;
            if (!User.IsInRole("Admin"))
            {
                ownerId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            }

            var reservations = await _reservationService.GetAllReservationsAsync(ownerId);
            return View(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _reservationService.CancelReservationAsync(id);
            if (result)
            {
                TempData["success"] = "Reservation cancelled successfully.";
            }
            else
            {
                TempData["error"] = "Failed to cancel reservation.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
