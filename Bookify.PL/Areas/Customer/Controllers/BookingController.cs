using Bookify.BL.Interfaces;
using Bookify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using Bookify.Models.ViewModels;

namespace Bookify.PL.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IPaymentService _paymentService;
        private readonly IRoomTypeService _roomTypeService;

        public BookingController(IReservationService reservationService, IPaymentService paymentService, IRoomTypeService roomTypeService)
        {
            _reservationService = reservationService;
            _paymentService = paymentService;
            _roomTypeService = roomTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync(r => r.Id == roomTypeId, includeProperties: "Hotel");
            var roomType = roomTypes.FirstOrDefault();

            var nights = (checkOutDate - checkInDate).Days;
            if (nights < 1) nights = 1;

            var basePrice = roomType.BasePrice * nights;
            var total = basePrice;

            var model = new CheckoutViewModel
            {
                RoomType = roomType,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalNights = nights,
                BasePrice = basePrice,
                Tax = 0,
                TotalPrice = total
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Challenge();

            try
            {
                var reservationId = await _reservationService.CreateReservationAsync(userId, roomTypeId, checkInDate, checkOutDate);
                var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(roomTypeId);

                if (reservation == null || roomType == null) return NotFound();

                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var session = _paymentService.CreateCheckoutSession(reservation, roomType, domain);

                if (string.IsNullOrEmpty(session.Url))
                {
                    TempData["error"] = "Failed to create payment session.";
                    return RedirectToAction("Checkout", new { roomTypeId, checkInDate, checkOutDate });
                }

                return Redirect(session.Url);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                // Redirect back to hotel page. We need hotelId.
                // Since we might not have it easily if roomType fetch failed, we try to fetch it again or just redirect to home.
                var roomType = await _roomTypeService.GetRoomTypeByIdAsync(roomTypeId);
                if (roomType != null)
                {
                    return RedirectToAction("GetHotel", "Hotel", new { id = roomType.HotelId });
                }
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Confirmation(int reservationId)
        {
            await _reservationService.ConfirmReservationAsync(reservationId, "Stripe_Session");
            return View(reservationId);
        }

        public async Task<IActionResult> Cancel(int reservationId)
        {
            await _reservationService.CancelReservationAsync(reservationId);
            return View(reservationId);
        }
    }
}
