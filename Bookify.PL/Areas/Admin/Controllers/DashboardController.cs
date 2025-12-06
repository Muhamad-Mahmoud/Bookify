using Bookify.BL.Interfaces;
using Bookify.Models;
using Bookify.Models.ViewModels;
using Bookify.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bookify.PL.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role + "," + SD.Owner_Role)]
    public class DashboardController : Controller
    {
        private readonly IHotelService _hotelService;
        private readonly IReservationService _reservationService;
        private readonly IInvoiceService _invoiceService;
        private readonly UserManager<Bookify.Models.Customer> _userManager;

        public DashboardController(
            IHotelService hotelService,
            IReservationService reservationService,
            IInvoiceService invoiceService,
            UserManager<Bookify.Models.Customer> userManager)
        {
            _hotelService = hotelService;
            _reservationService = reservationService;
            _invoiceService = invoiceService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            var isAdmin = User.IsInRole(SD.Admin_Role);

            // Fetch data
            var allHotels = await _hotelService.GetAllHotelsAsync(includeProperties: "RoomTypes.Rooms,Owner");
            var allReservations = await _reservationService.GetAllReservationsAsync();
            var allInvoices = await _invoiceService.GetAllInvoicesAsync();

            // Filter data based on role
            var hotels = isAdmin ? allHotels : allHotels.Where(h => h.Owner?.Id == userId).ToList();
            var hotelIds = hotels.Select(h => h.Id).ToHashSet();

            var reservations = isAdmin ? allReservations : allReservations.Where(r => hotelIds.Contains(r.HotelId)).ToList();
            var invoices = isAdmin ? allInvoices : allInvoices.Where(i => hotelIds.Contains(i.HotelId)).ToList();

            // Calculate Metrics
            var dashboardData = new DashboardVM
            {
                IsAdmin = isAdmin,
                OwnerEmail = user?.Email,
                TotalHotels = hotels.Count(),
                ActiveReservations = reservations.Count(r => r.Status == ReservationStatus.Pending || r.Status == ReservationStatus.Confirmed),
                AvailableRooms = hotels.SelectMany(h => h.RoomTypes ?? Enumerable.Empty<RoomType>())
                                       .SelectMany(rt => rt.Rooms ?? Enumerable.Empty<Room>())
                                       .Count(r => r.Status == RoomStatus.Available),
                TotalRevenue = invoices.Sum(i => i.InvoiceAmount),
                RecentReservations = reservations.OrderByDescending(r => r.CheckInDate).Take(5).ToList()
            };

            return View(dashboardData);
        }
    }
}
