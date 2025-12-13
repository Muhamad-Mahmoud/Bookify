using Bookify.Models;

namespace Bookify.DL.Repository.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetHotelReviewsAsync(int hotelId);
        Task<Review?> GetUserReviewForHotelAsync(string customerId, int hotelId);
        Task<bool> HasUserPaidForHotelAsync(string customerId, int hotelId);
    }
}
