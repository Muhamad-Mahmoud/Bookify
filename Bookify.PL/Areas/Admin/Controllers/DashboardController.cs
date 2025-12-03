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
        private readonly IRoomService _roomService;
        private readonly UserManager<Bookify.Models.Customer> _userManager;

        public DashboardController(
            IHotelService hotelService,
            IReservationService reservationService,
            IInvoiceService invoiceService,
            IRoomService roomService,
            UserManager<Bookify.Models.Customer> userManager)
        {
            _hotelService = hotelService;
            _reservationService = reservationService;
            _invoiceService = invoiceService;
            _roomService = roomService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId!);
            
            var isAdmin = User.IsInRole(SD.Admin_Role);
            var dashboardData = new DashboardVM
            {
                IsAdmin = isAdmin,
                OwnerEmail = user?.Email
            };

            var allHotels = await _hotelService.GetAllHotelsAsync(
                includeProperties: "RoomTypes.Rooms,Owner");
            
            var allReservations = await _reservationService.GetAllReservationsAsync();
            var allInvoices = await _invoiceService.GetAllInvoicesAsync();

            IEnumerable<Hotel> hotels;
            IEnumerable<Reservation> reservations;
            IEnumerable<Invoice> invoices;

            if (isAdmin)
            {
                hotels = allHotels;
                reservations = allReservations;
                invoices = allInvoices;
            }
            else
            {
                hotels = allHotels.Where(h => h.Owner.Id == user.Id);
                var ownerHotelIds = hotels.Select(h => h.Id).ToList();
                
                reservations = allReservations.Where(r => ownerHotelIds.Contains(r.HotelId));
                invoices = allInvoices.Where(i => ownerHotelIds.Contains(i.HotelId));
            }

            dashboardData.TotalHotels = hotels.Count();
            
            // Active reservations (Pending or Confirmed)
            dashboardData.ActiveReservations = reservations
                .Count(r => r.Status == ReservationStatus.Pending || 
                           r.Status == ReservationStatus.Confirmed);
            
            // Available rooms (get from loaded hotel data)
            dashboardData.AvailableRooms = hotels
                .SelectMany(h => h.RoomTypes ?? Enumerable.Empty<RoomType>())
                .SelectMany(rt => rt.Rooms ?? Enumerable.Empty<Room>())
                .Count(r => r.Status == RoomStatus.Available);
            
            // Total revenue from all invoices
            dashboardData.TotalRevenue = invoices.Any() ? invoices.Sum(i => i.InvoiceAmount) : 0;

            // Calculate month-over-month changes
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var lastMonth = currentMonth == 1 ? 12 : currentMonth - 1;
            var lastMonthYear = currentMonth == 1 ? currentYear - 1 : currentYear;

            // Reservations this month vs last month
            var thisMonthReservations = reservations
                .Count(r => r.CheckInDate.Month == currentMonth && r.CheckInDate.Year == currentYear);
            var lastMonthReservations = reservations
                .Count(r => r.CheckInDate.Month == lastMonth && r.CheckInDate.Year == lastMonthYear);
            
            if (lastMonthReservations > 0)
            {
                dashboardData.ReservationsChange = 
                    ((decimal)(thisMonthReservations - lastMonthReservations) / lastMonthReservations) * 100;
            }
            else
            {
                dashboardData.ReservationsChange = thisMonthReservations > 0 ? 100 : 0;
            }

            // Revenue this month vs last month
            var thisMonthRevenue = invoices
                .Where(i => i.IssuedAt.Month == currentMonth && i.IssuedAt.Year == currentYear)
                .Sum(i => (decimal?)i.InvoiceAmount) ?? 0;
            var lastMonthRevenue = invoices
                .Where(i => i.IssuedAt.Month == lastMonth && i.IssuedAt.Year == lastMonthYear)
                .Sum(i => (decimal?)i.InvoiceAmount) ?? 0;
            
            if (lastMonthRevenue > 0)
            {
                dashboardData.RevenueChange = ((thisMonthRevenue - lastMonthRevenue) / lastMonthRevenue) * 100;
            }
            else
            {
                dashboardData.RevenueChange = thisMonthRevenue > 0 ? 100 : 0;
            }

            // Available rooms and hotels change (placeholder - would need historical tracking)
            dashboardData.RoomsChange = 0;
            dashboardData.HotelsChange = 0;

            // Get recent reservations (last 5)
            dashboardData.RecentReservations = reservations
                .OrderByDescending(r => r.CheckInDate)
                .Take(5)
                .ToList();

            // Get recent invoices (last 5)
            dashboardData.RecentInvoices = invoices
                .OrderByDescending(i => i.IssuedAt)
                .Take(5)
                .ToList();

            return View(dashboardData);
        }
    }
}
