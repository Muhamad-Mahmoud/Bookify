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
    public class ReservationRepository : Repository<Reservation>, IReservationRepository
    {
        private readonly BookifyDbContext _dbContext;
        public ReservationRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Reservation reservation)
        {
            _dbContext.Reservations.Update(reservation);
        }
    }
}
