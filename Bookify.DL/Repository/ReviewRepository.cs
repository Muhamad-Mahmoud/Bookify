using Bookify.DL.Data;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookify.DL.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly BookifyDbContext _context;

        public ReviewRepository(BookifyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetHotelReviewsAsync(int hotelId)
        {
            return await _context.Reviews
                .Where(r => r.HotelId == hotelId)
                .Include(r => r.Customer)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<Review?> GetUserReviewForHotelAsync(string customerId, int hotelId)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.CustomerId == customerId && r.HotelId == hotelId);
        }

        public async Task<bool> HasUserPaidForHotelAsync(string customerId, int hotelId)
        {
            return await _context.Invoices
                .AnyAsync(i => i.CustomerId == customerId && 
                              i.HotelId == hotelId && 
                              i.PaidAt != null);
        }
    }
}
