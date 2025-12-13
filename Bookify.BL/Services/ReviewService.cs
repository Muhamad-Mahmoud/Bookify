using Bookify.BL.Interfaces;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;

namespace Bookify.BL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Review?> AddReviewAsync(Review review)
        {
            // Check if user can review
            if (!await CanUserReviewHotelAsync(review.CustomerId, review.HotelId))
            {
                return null;
            }

            // Check if user already reviewed this hotel
            var existingReview = await GetUserReviewForHotelAsync(review.CustomerId, review.HotelId);
            if (existingReview != null)
            {
                return null; // User already reviewed
            }

            review.CreatedAt = DateTime.Now;
            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveAsync();

            // Update hotel rating
            await UpdateHotelRatingAsync(review.HotelId);

            return review;
        }

        public async Task<IEnumerable<Review>> GetHotelReviewsAsync(int hotelId)
        {
            return await _unitOfWork.Reviews.GetHotelReviewsAsync(hotelId);
        }

        public async Task<bool> CanUserReviewHotelAsync(string customerId, int hotelId)
        {
            return await _unitOfWork.Reviews.HasUserPaidForHotelAsync(customerId, hotelId);
        }

        public async Task<Review?> GetUserReviewForHotelAsync(string customerId, int hotelId)
        {
            return await _unitOfWork.Reviews.GetUserReviewForHotelAsync(customerId, hotelId);
        }

        public async Task UpdateHotelRatingAsync(int hotelId)
        {
            var reviews = await GetHotelReviewsAsync(hotelId);
            var hotel = await _unitOfWork.Hotels.GetAsync(hotelId);

            if (hotel != null && reviews.Any())
            {
                hotel.UserRating = Math.Round(reviews.Average(r => r.Rating), 1);
                hotel.ReviewCount = reviews.Count();

                await _unitOfWork.SaveAsync();
            }
        }
    }
}
