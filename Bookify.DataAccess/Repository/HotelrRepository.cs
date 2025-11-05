using Bookify.DL.Data;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository
{
    public class HotelRepository : Repository<Hotel>, IHotelRepository
    {
        private readonly BookifyDbContext _dbContext;
        public HotelRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Hotel hotel)
        {
            _dbContext.Hotels.Update(hotel);
        }
    }
}
