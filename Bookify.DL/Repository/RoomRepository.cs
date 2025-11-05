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
    public class RoomRepository : Repository<Room>, IRoomRepository
    {
        private readonly BookifyDbContext _dbContext;
        public RoomRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Room room)
        {
            _dbContext.Rooms.Update(room);
        }
    }
}
