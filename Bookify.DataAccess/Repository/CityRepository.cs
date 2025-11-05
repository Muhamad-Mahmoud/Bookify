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
    public class CityRepository : Repository<City>, ICityRepository
    {
        private readonly BookifyDbContext _dbContext;
        public CityRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(City city)
        {
            _dbContext.Cities.Update(city);
        }
    }
}
