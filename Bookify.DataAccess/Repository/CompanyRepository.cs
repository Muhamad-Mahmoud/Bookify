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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly BookifyDbContext _dbContext;
        public CompanyRepository(BookifyDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public void Update(Company company)
        {
            _dbContext.Company.Update(company);
        }
    }
}
