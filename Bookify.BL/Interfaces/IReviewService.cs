using Bookify.Models;

namespace Bookify.BL.Interfaces
{
    public interface IReviewService
    {
        Task<Review?> AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetHotelReviewsAsync(int hotelId);
        Task<bool> CanUserReviewHotelAsync(string customerId, int hotelId);
        Task UpdateHotelRatingAsync(int hotelId);
        Task<Review?> GetUserReviewForHotelAsync(string customerId, int hotelId);
    }
}
