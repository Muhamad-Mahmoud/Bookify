using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookify.BL.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReservationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> CalculatePrice(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            var roomType = await _unitOfWork.RoomTypes.GetAsync(roomTypeId);
            if (roomType == null) return 0;

            var days = (checkOut - checkIn).TotalDays;
            if (days <= 0) return 0;

            return (decimal)days * roomType.BasePrice;
        }

        private async Task<Room?> FindAvailableRoomAsync(int roomTypeId, DateTime checkIn, DateTime checkOut)
        {
            // Get all rooms
            var rooms = await _unitOfWork.Rooms.GetAllAsync(r => r.RoomTypeId == roomTypeId);
            var roomIds = rooms.Select(r => r.Id).ToList();

            // Find overlapping reservations for this room type
            var overlappingReservations = await _unitOfWork.Reservations.GetAllAsync(
                r => r.Status != ReservationStatus.Cancelled
                     && r.CheckInDate < checkOut
                     && r.CheckOutDate > checkIn
                     && r.RoomReserved.Any(rr => rr.Room.RoomTypeId == roomTypeId),
                includeProperties: "RoomReserved"
            );

            // Identify booked room IDs
            var bookedRoomIds = overlappingReservations
                .SelectMany(r => r.RoomReserved)
                .Select(rr => rr.RoomId)
                .ToHashSet();

            // Return the first room that is not booked
            return rooms.FirstOrDefault(r => !bookedRoomIds.Contains(r.Id));
        }

        public async Task<int> CreateReservationAsync(string userId, int roomTypeId, DateTime checkInDate, DateTime checkOutDate)
        {
            // Find available room
            var availableRoom = await FindAvailableRoomAsync(roomTypeId, checkInDate, checkOutDate);

            var roomType = await _unitOfWork.RoomTypes.GetAsync(roomTypeId);

            // Calculate total price
            var totalPrice = await CalculatePrice(roomTypeId, checkInDate, checkOutDate);

            // Create Reservation entity
            var reservation = new Reservation
            {
                CustomerId = userId,
                Status = ReservationStatus.Pending,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                TotalPrice = totalPrice,
                PaymentMethod = "Pending",
                HotelId = roomType.HotelId,
                RoomReserved = new List<ReservedRoom>()
            };

            // Save Reservation
            await _unitOfWork.Reservations.AddAsync(reservation);
            await _unitOfWork.SaveAsync();

            // Create ReservedRoom
            var reservedRoom = new ReservedRoom
            {
                RoomId = availableRoom.Id,
                ReservationId = reservation.Id
            };

            await _unitOfWork.reservedRooms.AddAsync(reservedRoom);
            await _unitOfWork.SaveAsync();


            return reservation.Id;
        }

        public async Task<bool> ConfirmReservationAsync(int reservationId, string paymentIntentId)
        {
            var reservation = await _unitOfWork.Reservations.GetAsync(
                r => r.Id == reservationId,
                includeProperties: "RoomReserved.Room.RoomType"
            );
            if (reservation == null) return false;

            reservation.Status = ReservationStatus.Confirmed;
            reservation.PaymentMethod = "Stripe";

            // Update Room Status to Occupied
                // Removed updating Room Status to Occupied
                // Room status should reflect physical availability (e.g., Maintenance), not booking status.
                // Booking availability is handled by checking overlapping reservations.

            // Create Invoice
            var invoice = new Invoice
            {
                CustomerId = reservation.CustomerId,
                ReservationId = reservation.Id,
                InvoiceAmount = reservation.TotalPrice,
                IssuedAt = DateTime.Now,
                PaidAt = DateTime.Now,
                Status = InvoiceStatus.Paid,
                HotelId = reservation.HotelId,
                PaymentIntentId = paymentIntentId
            };

            await _unitOfWork.Invoices.AddAsync(invoice);

            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _unitOfWork.Reservations.GetAsync(
                r => r.Id == reservationId,
                includeProperties: "RoomReserved.Room.RoomType,Customer,Hotel"
            );
        }

        public async Task<IEnumerable<Reservation>> GetReservationsForUserAsync(string userId)
        {
            return await _unitOfWork.Reservations.GetAllAsync(
                r => r.CustomerId == userId,
                includeProperties: "RoomReserved.Room.RoomType,Hotel"
            );
        }

        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync(string? ownerId = null)
        {
            if (string.IsNullOrEmpty(ownerId))
            {
                return await _unitOfWork.Reservations.GetAllAsync(includeProperties: "RoomReserved.Room.RoomType.Hotel,Customer,Hotel");
            }
            else
            {
                // Filter reservations directly by HotelId via navigation property
                return await _unitOfWork.Reservations.GetAllAsync(
                    r => r.Hotel.OwnerId == ownerId,
                    includeProperties: "RoomReserved.Room.RoomType.Hotel,Customer,Hotel"
                );
            }
        }

        public async Task<bool> CancelReservationAsync(int reservationId)
        {
            var reservation = await _unitOfWork.Reservations.GetAsync(reservationId);
            if (reservation == null) return false;

            // Prevent cancellation of confirmed reservations
            if (reservation.Status == ReservationStatus.Confirmed)
            {
                throw new InvalidOperationException("Cannot cancel a confirmed reservation.");
            }

            reservation.Status = ReservationStatus.Cancelled;
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}