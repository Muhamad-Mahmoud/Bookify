using Bookify.DL.Data;
using Bookify.DL.Repository.IRepository;
using Bookify.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookifyDbContext _dbContext;

        public ICityRepository Cities { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public ICountryRepository Countries { get;private set; }
        public ICustomerRepository Customers { get;private set; }
        public IHotelRepository Hotels { get;private set; }
        public IInvoiceRepository Invoices { get;private set; }
        public IReservationRepository Reservations { get;private set; }
        public IRoomRepository Rooms { get;private set; }
        public IRoomTypeRepository RoomTypes { get;private set; }

        public UnitOfWork(BookifyDbContext dbContext)
        {
            _dbContext = dbContext;

            Cities = new CityRepository(_dbContext);
            Companies = new CompanyRepository(_dbContext);
            Countries = new CountryRepository(_dbContext);
            Customers = new CustomerRepository(_dbContext);
            Hotels = new HotelRepository(_dbContext);
            Invoices = new InvoiceRepository(_dbContext);
            Reservations = new ReservationRepository(_dbContext);
            Rooms= new RoomRepository(_dbContext);
            RoomTypes = new RoomTypeRepository(_dbContext);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}
