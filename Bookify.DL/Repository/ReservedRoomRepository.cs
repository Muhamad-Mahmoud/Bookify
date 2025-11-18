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
    public class ReservedRoomRepository : Repository<ReservedRoom>, IReservedRoomRepository
    {
        private readonly BookifyDbContext _dbContext;
        public ReservedRoomRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(ReservedRoom reservedRoom)
        {
            _dbContext.ReservedRooms.Update(reservedRoom);
        }
    }
}
