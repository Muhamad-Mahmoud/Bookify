using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
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
        public async Task<decimal> CalculatePrice(int roomId, DateTime startDate, DateTime endDate)
        {
            var room = await _unitOfWork.Rooms.GetAsync(roomId);

            var days = (endDate - startDate).TotalDays;

            if (days <= 0)
                return 0;

            var totalPrice = (decimal)days * room.Price;
            return totalPrice;
        }

        public async Task<bool> CheckAvailabilityAsync(int roomId, DateTime date)
        {
            // Normalize the input date to date-only
            var targetDate = date.Date;

            // Check for any reservation that includes the target date.
            // Here we treat CheckOutDate as exclusive (guest leaves on CheckOutDate morning).
            // Note: comparing DateTime properties without .Date helps EF Core translate the
            // expression to SQL; we normalize the input instead.
            var reservation = await _unitOfWork.Reservations.GetAsync(
                r => r.RoomReserved.Any(rr => rr.RoomId == roomId)
                     && r.CheckInDate <= targetDate
                     && r.CheckOutDate > targetDate
            );

            return reservation == null;
        }

        public Task<int> CreatePendingReservationAsync(int roomId, string userId)
        {
            var room = _unitOfWork.Rooms.GetAsync( roomId);
            var user = _unitOfWork.Customers.GetAsync(u=>u.Id==userId);

            //var reservation = new Reservation
            //{
            //    CustomerId = userId,
            //    Status = "pending",
            //    CheckInDate = DateTime.Now,
            //    TotalPrice = CalculatePrice(roomId, )
            //}
            
            
            
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmReservationAsync(int reservationId, string paymentIntentId)
        {
            throw new NotImplementedException();
        }

        public Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Reservation>> GetReservationsForUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelReservationAsync(int reservationId)
        {
            throw new NotImplementedException();
        }
    }
}