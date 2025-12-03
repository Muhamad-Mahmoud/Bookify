using Bookify.DL.Data;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository
{
    public class HotelImageRepository : Repository<HotelImage>, IHotelImageRepository
    {
        private readonly BookifyDbContext _dbContext;

        public HotelImageRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(HotelImage hotelImage)
        {
            _dbContext.HotelImages.Update(hotelImage);
        }
    }
}
