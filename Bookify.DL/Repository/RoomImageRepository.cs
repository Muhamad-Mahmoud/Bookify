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
    public class RoomImageRepository : Repository<RoomImage>, IRoomImageRepository
    {
        private readonly BookifyDbContext _dbContext;

        public RoomImageRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(RoomImage roomImage)
        {
            _dbContext.RoomImages.Update(roomImage);
        }
    }
}
