using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BL.Interfaces
{
    public interface IReservationService
    {
        Task<int> CreateReservationAsync(string userId, int roomTypeId, DateTime checkInDate, DateTime checkOutDate);
        Task<decimal> CalculatePrice(int roomTypeId, DateTime checkIn, DateTime checkOut);

        Task<bool> ConfirmReservationAsync(int reservationId, string paymentIntentId);
        Task<bool> CancelReservationAsync(int reservationId);

        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<IEnumerable<Reservation>> GetReservationsForUserAsync(string userId);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync(string? ownerId = null);
    }
}
