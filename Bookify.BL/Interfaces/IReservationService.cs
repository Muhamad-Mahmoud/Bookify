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
        // Pre-payment operations
        Task<bool> CheckAvailabilityAsync(int roomId, DateTime date);
        Task<int> CreatePendingReservationAsync(int roomId, string userId);
        Task<decimal> CalculatePrice(int roomId, DateTime startDate, DateTime endDate);

        
        // Post-payment operations
        Task<bool> ConfirmReservationAsync(int reservationId, string paymentIntentId);
        Task<bool> CancelReservationAsync(int reservationId);

        
        // Retrieval operations
        Task<Reservation?> GetReservationByIdAsync(int reservationId);
        Task<IEnumerable<Reservation>> GetReservationsForUserAsync(string userId);
    }
}
